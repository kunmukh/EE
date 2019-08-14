#include<stdio.h>
void DrawCircle(void); //function prototype
void DrawTriangle(void); 
void DrawIntersectingLines(void);
void DrawBase(void);

int main()
{
    DrawCircle();
    DrawTriangle();
}
void DrawCircle(void) // fnction definition
{
    printf("   * *\n");
    printf("  *   *\n");
    printf("   * *\n");
}
void DrawTriangle(void)
{
    DrawIntersectingLines();
    DrawBase();
}
void DrawIntersectingLines(void)
{
    printf("  /\\ \n");
    printf(" /  \\ \n");
    printf("/    \\ \n");
}
void DrawBase(void)
{
    printf("------\n");
}