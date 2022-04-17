#include "TimerManage.h"

TimerManage::TimerManage(QWidget *parent)
    : QMainWindow(parent)
{
    ui.setupUi(this);
    connect(ui.pushButton_start, &QPushButton::clicked, this, &TimerManage::Btn_Start);
    connect(ui.pushButton_finish, &QPushButton::clicked, this, &TimerManage::Btn_Finish);
    connect(ui.pushButton_chart, &QPushButton::clicked, this, &TimerManage::Btn_Chart);
    connect(ui.pushButton_list, &QPushButton::clicked, this,& TimerManage::Btn_List);

    ui.pushButton_finish->setEnabled(false);

    time = new QTimer(this);
    connect(time, &QTimer::timeout, this, &TimerManage::Timer_Update);
}

TimerManage::~TimerManage()
{

}

void TimerManage::Btn_Start()
{
    if (!_startflag) {
        ui.pushButton_start->setText(QStringLiteral("暂停"));
        time->start(1000);
        _startflag = true;
    }
    else {
        ui.pushButton_start->setText(QStringLiteral("开始"));
        time->stop();
        if (QMessageBox::information(this, QStringLiteral("提示"), QStringLiteral("五秒后继续计时!"), QMessageBox::Ok) == QMessageBox::Ok)
        {
            time->start(1000);
            _startflag = false;
        }

    }
}

void TimerManage::Btn_Finish()
{
    ui.label_title->setText(QStringLiteral("结束"));
}

void TimerManage::Btn_Chart()
{
}

void TimerManage::Btn_List()
{
}

void TimerManage::Timer_Update()
{
    if (_startflag == true) {
        if (_secs == 59) {
            _mins++;        //min = min + 1;
            _secs = 0;
        }
        else {
            _secs++;
        }

        if (_mins == 59) {
            _hours++;        //min = min + 1;
            _mins = 0;
        }

        QString str = QString("%1:%2:%3").arg(_hours, 4, 10, QLatin1Char('0')).arg(_mins, 2, 10, QLatin1Char('0')).arg(_secs, 2, 10, QLatin1Char('0'));
        ui.label_countTime->setText(str);
    }
    else {
        if (_pauseflag < 5) {
            _pauseflag++;
            QMessageBox::information(this, QStringLiteral("提示"), QString::number(_pauseflag));
        }
    }

}
