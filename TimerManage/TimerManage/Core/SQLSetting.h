#pragma once
#include <QWidget>
#include <QSqlDatabase>
#include <QSqlError>
#include <QSqlQuery>

class SQLSetting : public QObject
{
    Q_OBJECT

public:
    SQLSetting();
    ~SQLSetting();

private:
   QSqlDatabase _database;

private:
    void InitDataBase();

};

