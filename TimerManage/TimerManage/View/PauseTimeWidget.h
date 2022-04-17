#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_PauseTimeWidget.h"

class PauseTimeWidget : public QMainWindow
{
    Q_OBJECT

public:
    PauseTimeWidget(QWidget* parent = Q_NULLPTR);
    ~PauseTimeWidget();

    void ShowTime(int time);

private:
    Ui::PauseTimeWidget ui;
};