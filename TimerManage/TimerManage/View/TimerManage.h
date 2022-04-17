#pragma once

#include <QtWidgets/QMainWindow>
#include <QTimer>
#include <QMessageBox>
#include "ui_TimerManage.h"
#include "PauseTimeWidget.h"

class TimerManage : public QMainWindow
{
    Q_OBJECT

public:
    TimerManage(QWidget *parent = Q_NULLPTR);
    ~TimerManage();

private:
    Ui::TimerManageClass ui;
    QTimer *time;
    QSqlDatabase* database;

    bool _startflag = false;
    int _hours = 0, _mins = 0, _secs = 0;
    int _pauseflag = 0;

    std::shared_ptr<PauseTimeWidget> _pauseWidget = nullptr;

private:
    void InitDataBase();

private slots:
    void Btn_Start();
    void Btn_Finish();
    void Btn_Chart();
    void Btn_List();

    void Timer_Update();
};
