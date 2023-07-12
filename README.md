# Yinyang Utilities MySql

[![Yinyang Utilities MySql](https://img.shields.io/nuget/v/Yinyang.Utilities.MySql.svg)](https://www.nuget.org/packages/Yinyang.Utilities.MySql/)

MySql, MariaDB Connection Utility for C#.

C#用MySql,MariaDB接続ユーティリティです。

---

## Getting started

Install Yinyang Utilities MySql nuget package.

NuGet パッケージ マネージャーからインストールしてください。

- [MySql](https://www.nuget.org/packages/Yinyang.Utilities.MySql/)

> ```powershell
> Install-Package Yinyang.Utilities.MySql
> ```

---

## Basic Usage

```c#
// Init
using var db = new MySqlConnect(ConnectionString);

// Database Open
db.Open();

// Transaction Start
db.BeginTransaction();

// SQL
db.CommandText = "INSERT INTO test VALUES(@id, @value)";

// Add Parameter
db.AddParameter("@id", 1);
db.AddParameter("@value", "abcdefg");

// Execute
if (1 != db.ExecuteNonQuery())
{
    // Transaction Rollback
    db.Rollback();
    return;
}

// Command and Parameter Reset
db.Refresh();

// SQL
db.CommandText = "select * from test where id = @id;";

// Add Parameter
db.AddParameter("@id", 1);

// Execute
var result = db.ExecuteReaderFirst<Entity>();

if (null == result)
{
    db.Rollback();
    return;
}

// Transaction Commit
db.Commit();

// Database Close
db.Close();


```

## Samples

See Sample project.
