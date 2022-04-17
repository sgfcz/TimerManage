#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_TimerManage.h"

class TimerManage : public QMainWindow
{
    Q_OBJECT

public:
    TimerManage(QWidget *parent = Q_NULLPTR);

private:
    Ui::TimerManageClass ui;
};
