#include <stdio.h>
#include "LineLib.h"

int main()
{
 double x1, y1, x2, y2;
 double dist, slope;
 x1 = 0; y1 = 0;
 x2 = 1.0; y2 = 1.0;
 dist = FindDistance(x1, y1, x2, y2);
 slope = FindSlope(x1, y1, x2, y2);
 printf("Line from (%4.1f, %4.1f) to (%4.1f, %4.1f)\n",
 x1, y1, x2, y2);
 printf("has a length of %6.2f\n", dist);
 printf("and a slope of %6.2f\n", slope);
 return 0;
}