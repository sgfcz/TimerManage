#include "./View/TimerManage.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    TimerManage w;
    w.show();
    return a.exec();
}
