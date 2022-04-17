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
        // ������SQlite���ݿ������
        _database = QSqlDatabase::addDatabase("QSQLITE");
        // �������ݿ��ļ�������
        _database.setDatabaseName("MyDataBase.db");
    }
}
