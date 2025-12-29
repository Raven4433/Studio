using UnityEngine;
using SimpleSQL;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Studio.Data {

public static class DBTools {

    // --- ID MANAGEMENT ---

    /// <summary>
    /// Gets the next available ID for a table by checking the max existing ID.
    /// Assumes table has an integer Primary Key named "ID".
    /// </summary>
    public static int GetNewID<T>(SimpleSQLManager db) where T : new() {
        if(db == null) return 1;
        
        string tableName = typeof(T).Name;
        // Scalar query for max ID logic
        // Note: SimpleSQL QueryGeneric returns list. 
        // We select the ID column, order desc, limit 1.
        
        var query = $"SELECT ID FROM {tableName} ORDER BY ID DESC LIMIT 1";
        var result = db.Query<T>(query);
        
        if(result != null && result.Count > 0){
            // Use reflection to get the ID property value
            var prop = typeof(T).GetProperty("ID");
            if(prop != null){
                int currentMax = (int)prop.GetValue(result[0]);
                return currentMax + 1;
            }
        }
        
        return 1; // Start at 1 if table empty
    }

    // --- CRUD WRAPPERS ---

    public static void InsertOrUpdate<T>(SimpleSQLManager db, T item) {
        if(db == null || item == null) return;
        db.Insert(item);
    }

    public static void Delete<T>(SimpleSQLManager db, object primaryKey) where T : new() {
        if(db == null) return;
        string tableName = typeof(T).Name;
        db.Execute($"DELETE FROM {tableName} WHERE ID = ?", primaryKey);
    }

    // --- TRANSACTIONS ---

    public static void ExecuteTransaction(SimpleSQLManager db, Action action) {
        if(db == null) return;
        try {
            db.BeginTransaction();
            action?.Invoke();
            db.Commit();
        } catch (Exception e) {
            Debug.LogError($"[DBTools] Transaction failed: {e.Message}");
            // Optional: db.Rollback(); (Check docs if exposed)
        }
    }

    // --- QUERIES ---

    // public static int Count<T>(SimpleSQLManager db) {
    //     if(db == null) return 0;
    //     string tableName = typeof(T).Name;
    //     // SimpleSQL allows scalar queries via ExecuteScalar if available, otherwise select count
    //     // For generic T, we query list and count it (slower but universal) or raw SQL
        
    //     // Option A: Raw SQL (Fastest)
    //     // var count = db.ExecuteScalar<int>($"SELECT COUNT(*) FROM {tableName}"); // If SimpleSQL supports generic scalar
        
    //     // Option B: List (Safe)
    //     var list = db.Query<T>($"SELECT ID FROM {tableName}");
    //     return list.Count;
    // }


    
}
}