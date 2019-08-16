//Kunal Mukherjee
//CS 210
//CS 210- Strings
//20th Oct, 2016

#include<stdio.h>
#include<string.h>

int main ()
{
    int size = 26;
    char alpha[] = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
    int cnt[26];
    for (int i=0; i <26; i++)
        cnt[i]=0;

    char line[80];
    int i;
    printf("Enter a line of data... \n");
    gets(line);
    

    for (i =0; i < sizeof(line); i++){
    if (line[i] >= 'a' && line[i] <='z')
    {
        cnt[line[i]-'a']++;
    }
    }
    for (i =0; i < size; i++)
    {
    printf("%c = %d \n",alpha[i] ,cnt[i] );
    }
    
    }
