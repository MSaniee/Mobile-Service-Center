﻿using ServiceCenter.Application.Interfaces.Repositories;
using ServiceCenter.Common;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ServiceCenter.Infrastructure.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly ApplicationDbContext DbContext;
    public DbSet<TEntity> Entities { get; }
    public virtual IQueryable<TEntity> Table => Entities;
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    public Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        Entities = DbContext.Set<TEntity>(); // City => Cities
    }

    public IQueryable<T> GetTable<T>(bool enableTracking = true) where T : class, IEntity =>
        enableTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();

    #region Async Method

    public virtual async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return await Entities.FindAsync(ids, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Update(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity> UpdateAsNoTrackingAsync<TKey>(BaseEntity<TKey> entity, CancellationToken cancellationToken, bool saveNow = true)
    {

        var entry = Entities.Local.FirstOrDefault(e => (e as BaseEntity<TKey>).Id.Equals(entity.Id));

        if (entry is null)
        {
            DbContext.Attach(entry);
            DbContext.Entry(entry).State = EntityState.Modified;
        }
        else
        {
            DbContext.Entry(entry).CurrentValues.SetValues(entity);
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        Entities.UpdateRange(entities);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Remove(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        Entities.RemoveRange(entities);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task SaveChangeAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion Async Method

    #region Sync Methods

    public virtual TEntity GetById(params object[] ids)
    {
        return Entities.Find(ids);
    }

    public virtual void Add(TEntity entity, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Add(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        Entities.AddRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Update(TEntity entity, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Update(entity);
        DbContext.SaveChanges();
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        Entities.UpdateRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Delete(TEntity entity, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Remove(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        Assert.NotNull(entities, nameof(entities));
        Entities.RemoveRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void SaveChange()
    {
        DbContext.SaveChanges();
    }

    #endregion Sync Methods

    #region Attach & Detach

    public virtual void Detach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        var entry = DbContext.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }

    public virtual void Attach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        if (DbContext.Entry(entity).State == EntityState.Detached)
            Entities.Attach(entity);
    }

    #endregion Attach & Detach

    #region Explicit Loading

    public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);

        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class
    {
        Attach(entity);
        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            collection.Load();
    }

    public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);
        var reference = DbContext.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class
    {
        Attach(entity);
        var reference = DbContext.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            reference.Load();
    }



    #endregion Explicit Loading
}