#include "SQLSetting.h"

SQLSetting::SQLSetting()
{
    InitDataBase();
}

SQLSetting::~SQLSetting()
{
}

void SQLSetting::InitDataBase()
{
    if (QSqlDatabase::contains("qt_sql_default_connection"))
    {
        _database = QSqlDatabase::database("qt_sql_default_connection");
    }
    else
    {
        // 建立和SQlite数据库的连接
        _database = QSqlDatabase::addDatabase("QSQLITE");
        // 设置数据库文件的名字
        _database.setDatabaseName("MyDataBase.db");
    }
}
