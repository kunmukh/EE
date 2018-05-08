#include <sys/types.h> 
#include <stdio.h> 
#include <unistd.h>

 int value = 5;

int main(){
  pid_t pid, pid1;

  pid = fork();

  if (pid< 0){
    fprintf (stderr , "Fork Failed");
    return 1;
  }
  else if (pid== 0){
    pid1 = getpid();
    printf("child: pid A= %d\n", pid);
    printf("child: pid1 B= %d\n" , pid1);
    
  }
  else{
    pid1 = getpid();
    printf("parent: pid C= %d\n", pid);
    printf("parent: pid1 D= %d\n" , pid1);
    wait(NULL);
  }

  return 0;
    
}
	   
	      
