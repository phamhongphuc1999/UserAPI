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

using System.Data.SQLite;
using UserAPI.Connecter;

namespace UserAPI.Services.SQLiteService
{
    public class TemplateSetvice: BaseService
    {
        public override void CreateTable(SQLiteConnecter connecter)
        {
            string createCommand = string.Format("CREATE TABLE IF NOT EXISTS Template {0}, {1}, {2}, {3})",
                "([id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT", "Name varchar(50)", "Type varchar(50)", "Description varchar(100)");
            SQLiteCommand command = new SQLiteCommand(createCommand, connecter.connection);
            command.ExecuteNonQuery();
        }
    }
}
