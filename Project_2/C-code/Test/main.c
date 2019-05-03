#include <stdio.h>
#include <stdlib.h>

int main()
{
    char data[8] = "55";

    for (int i = 0; i < 8; i++)
    {
        printf("%d", (int)data);
    }
    printf("\n");
    return 0;
}
