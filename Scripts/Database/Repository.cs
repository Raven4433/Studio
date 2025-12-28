using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleSQL;
using UnityEngine;

namespace Studio.Data {

// Generic Repository that works with ANY class having an 'ID' property (PK)
public class Repository<T> where T : class, new() {

    private SimpleSQLManager _db;
    private Dictionary<int, T> _cache = new Dictionary<int, T>();
    private PropertyInfo _idProp;
    
    // Events
    public event Action<T> OnUpdated;
    public event Action<T> OnAdded;
    public event Action<T> OnDeleted;
    public event Action OnReloaded;

    public Repository(SimpleSQLManager db){
        _db = db;
        // Cache the ID property info for speed
        _idProp = typeof(T).GetProperty("ID");
        if(_idProp == null){ Debug.LogError($"[Repository] Class {typeof(T).Name} has no 'ID' property!"); }
    }

    public T Get(int id){
        if(_cache.TryGetValue(id, out T item)){ return item; }

        string query = $"SELECT * FROM {typeof(T).Name} WHERE ID = ?";
        var dbItem = _db.Query<T>(query, id).FirstOrDefault();
        
        if(dbItem != null){ _cache[id] = dbItem; }
        return dbItem;
    }

    public List<T> GetAll(){
        // Simple "Lazy Load All" strategy
        if(_cache.Count == 0){ RefreshCache(); }
        return _cache.Values.ToList();
    }

    public void Save(T item){
        if(_idProp == null){ return; }
        
        int id = (int)_idProp.GetValue(item);
        bool isNew = (id == 0);
        
        _db.UpdateTable(item);
        
        // Re-fetch ID after insert (SQLite auto-increments it)
        id = (int)_idProp.GetValue(item);
        _cache[id] = item;
        
        if(isNew){ OnAdded?.Invoke(item); }
        else{ OnUpdated?.Invoke(item); }
    }

    public void Delete(T item){
        if(_idProp != null){
            int id = (int)_idProp.GetValue(item);
            if(_cache.ContainsKey(id)){ _cache.Remove(id); }
        }
        _db.Delete(item);
        OnDeleted?.Invoke(item);
    }

    public void RefreshCache(){
        _cache.Clear();
        var all = _db.Table<T>().ToList();
        foreach(var item in all){
            int id = (int)_idProp.GetValue(item);
            _cache[id] = item;
        }
        OnReloaded?.Invoke();
    }
}
}