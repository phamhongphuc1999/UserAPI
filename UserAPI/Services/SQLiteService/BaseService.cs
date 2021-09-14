// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using UserAPI.Connecter;
using System.Data.SQLite;

namespace UserAPI.Services.SQLiteService
{
    public class BaseService
    {
        public virtual void CreateTable(SQLiteConnecter connecter)
        {
        }

        protected virtual bool ExecuteCommand(string commandText, SQLiteConnection connecter)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, connecter);
            return command.ExecuteNonQuery() > 0;
        }

        protected virtual object ExecuteScalar(string commandText, SQLiteConnection connecter)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, connecter);
            return command.ExecuteScalar();
        }

        protected virtual SQLiteDataReader ExecuteReader(string commandText, SQLiteConnection connecter)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, connecter);
            return command.ExecuteReader();
        }
    }
}
