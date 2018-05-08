//Kunal Mukherjee
//Connected Components Algorithm-Heuristic
//4/12/2018

#include "stdio.h"
#include "stdlib.h"
#include "stdbool.h"

typedef struct vertex vertex;
struct vertex
{
  int vertexLabel;
  int rank;
  vertex * parent;  
};

void ConnectedComponent(vertex * forest,			
			int numVertices);

vertex *  Find(vertex * v);

void Union(vertex * v1, vertex * v2);

void Link(vertex * v1, vertex * v2);

int main (int argc, char **argv)
{
  
  int numVertices, numEdges, numTestcase;   
  FILE *file;
  file = fopen(argv[1],"r"); 
  fscanf(file, "%d", &numTestcase);
  
  vertex * forest; 

  for (int i = 0; i < numTestcase; i++)
    {
      fscanf(file, "%d", &numVertices);
      fscanf(file, "%d", &numEdges);    
            
      
      forest = malloc((numVertices + 1) * sizeof(vertex));            

      //Make-Set
      for (int i = 1; i <= numVertices; i++)
	{
	  forest[i].vertexLabel = i;
	  forest[i].parent = &forest[i];	  
	  forest[i].rank = 0;	    
	}
       

      int edge1, edge2;
      
     
      for (int j = 0; j < numEdges; j++)
	{ 
	  
	  fscanf(file, "%d", &edge1);
	  fscanf(file, "%d", &edge2); 
	 
	  //Find-Set
	  if( (Find(&forest[edge1]))->vertexLabel != (Find(&forest[edge2]))->vertexLabel)
	    {
	      //Union
	      Union(&forest[edge1],&forest[edge2]);	     
	    }  		  
	  	  
	}   
      
      ConnectedComponent(forest, numVertices);         
           
      free(forest);      
      printf("\n\n");
      
    }    
	
  fclose(file);  
 
  return 0;
}

vertex * Find(vertex * v)
{
  if (v->vertexLabel != v->parent->vertexLabel)
    {
      return v->parent = Find(v->parent);      
    }  
  return v->parent;
    
}

void Union(vertex * v1, vertex * v2)
{
  Link(Find(v1),Find(v2));
}

void Link(vertex * v1, vertex * v2)
{
  if (v1->rank > v2->rank)
    {
      v2->parent = v1->parent;
    }
  else
    {
      v1->parent = v2->parent;
      if(v1->rank == v2->rank)
	{
	  v2->rank = v2->rank +1;
	}
    }
}

void ConnectedComponent(vertex * forest,						
			int numVertices)
{
 printf("[ ");
  
  for (int i = 1; i <= numVertices; i++)
    {   
      if(forest[i].parent->vertexLabel == forest[i].vertexLabel)
	{
	  printf("[ ");
	  for (int j = 1; j <= numVertices; j++)
	    {
	      if ((Find(&forest[j]))->vertexLabel == forest[i].vertexLabel)
		{
		  printf("%d ",j);
		}
	    }
	  printf("]");
	}
    }    
  
  printf("]");   
}





		      
		    
		    	  	
	  
		 
    

