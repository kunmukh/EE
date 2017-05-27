#include "LineLib.h"
#include <math.h>
double FindDistance(double x1,double y1,double x2,double y2)
{return sqrt(pow(x1-x2, 2)+pow(y1-y2, 2));
}
double FindSlope(double x1,double y1,double x2,double y2)
{return (y2 - y1)/(x2 - x1);
}