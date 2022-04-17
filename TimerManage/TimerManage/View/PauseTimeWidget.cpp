#include "PauseTimeWidget.h"

PauseTimeWidget::PauseTimeWidget(QWidget* parent)
{
    ui.setupUi(this);
    setWindowFlags(Qt::FramelessWindowHint);
    setAttribute(Qt::WA_QuitOnClose, false);
}

PauseTimeWidget::~PauseTimeWidget()
{

}

void PauseTimeWidget::ShowTime(int time)
{
    this->show();
    ui.label_time->clear();
    ui.label_time->setText(QString::number(time));
}

void PauseTimeWidget::CloseTime()
{
    this->close();
}
