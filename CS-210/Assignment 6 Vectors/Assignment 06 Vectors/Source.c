// File: Asn06
// Kunal Mukherjee
// CS 210
// Assignment : Vectors
// The program will read a file which contains 20 pair of points. My program will provide a menu to the user allowing her to select one of five options: 1) points closest together 2) points furthest apart 3) triangle with the smallest area, 4) triangle with the largest area 5) exit.

#include<stdio.h> // This is the header file
#include<stdlib.h> 
#include<math.h>
#pragma warning(disable:4996) // The pragma warning for not using open_s

int    MenuPrint(); //This fucntion is going to print the menu and it returns nothing.
double Area(double a, double b, double c); //This function takes in three double side lenths that would be used to calculate the area of the triangle and it returns the area
double Distance(double x1, double y1, double x2, double y2);//This function takes in the two double points and calculate the distance between them. 
double LineLength( double const pointsarray[][2], int number_of_points, int largest_or_smallest_line, double point1[][2], double point2[][2]); //This fuctions takes the points array that has all the scaned points from the file, it also has the largest or smallest line that is used to decide is it finding the max or min length.Then it has the two double point vector that is used to store the calculated x and Y point for the respective max and min values. 
double AreaDifferenciator ( double const pointsarray[][2], int number_of_points, int largest_or_smallest_area, double vertex1[][2], double vertex2[][2],double vertex3[][2] );//This fuctions takes the points array that has all the scaned points from the file, it also has the largest or smallest area differentiator value that is used to decide is it finding the max or min area.Then it has the three double vertex vector that is used to store the calculated x and Y point for the respective max and min values of the area. 


int main() //The main function

{
   double  pointsarray[20][2]; // The vector definition of pointsarray

   double point1[1][2], point2[1][2]; // The vector definition of points1 and points2 so that they have 1 row and 2 column. 

   double vertex1[1][2], vertex2[1][2], vertex3[1][2];// The vector definiton of vertex1 , vertex2 and vertex4, so that they have 1 row and 2 column.

   double x1,x2,y1,y2,x3,y3; // double to store the three pairs cordinate points

   int choice = 0;  // initializing choice 

   int number_of_points = 20;  // Total number of points initialized 
   {
               double x[20], y[20];  // Intilizing the vector x and y so that it has 20 elements in the vector

               FILE *point; 

               point = fopen("Asn06.txt", "r");

               for (int i = 0; i < number_of_points; i++) //scaning the points in Asn06.txt to the array x and y

                    {
                        fscanf_s( point , "%lf,%lf" , &x[i] , &y[i] );
                    }            

               for (int i=0 ; i < 20; i++) // The points in x and y is being combined into a single vector called pointsarray for easier calculation

                    {   
                        pointsarray[i][0] = x[i];

                        pointsarray[i][1] = y[i];
                    }    
    }                   

     printf("\nThe points in the designated file are :\n");

     for (int i=0 ; i < 20; i++)

                    {   
                        
                        printf(" Point %d - %lf, %lf\n",i, pointsarray[i][0],pointsarray[i][1] );
                    }    

        choice = MenuPrint();         
        
      while (choice !=5 ) // if the choice is 5 it ends

        {
               if    (choice == 1 || choice == 2){ 
                int largest_or_smallest_line = choice; //The choice is the largest_or_smallest_line , so that it can be used to differentiate the max or min lenght

                double line_length ;

               line_length =  LineLength (pointsarray, number_of_points, largest_or_smallest_line, point1, point2);

                x1 = point1[0][0]; 
    
                y1=  point1[0][1];

                x2 = point2[0][0];

                y2 = point2[0][1];                

                printf("Points are %5.2f, %5.2f and %5.2f, %5.2f and the Distance between them are %5.2f.\n",x1,y1,x2,y2,line_length);  
                
                choice = MenuPrint();  }  
                        
                       
      
      if (choice == 3 || choice == 4 )
      {
            double area;

            int largest_or_smallest_area = choice;

            area = AreaDifferenciator (pointsarray, number_of_points, largest_or_smallest_area, vertex1, vertex2, vertex3 ); // This is calling the function AreaDifferentiator to find the max or min values, it is passing the array points array and the three vertex array, the max_or_min_area_differentiator and the max point.

            x1 = vertex1[0][0];

            y1=  vertex1[0][1];

            x2 = vertex2[0][0];

            y2 = vertex2[0][1];

            x3 = vertex3[0][0];           

            y3 = vertex3[0][1];

            printf("Points are %5.2f, %5.2f, %5.2f, %5.2f and %5.2f,%5.2f and the corresponding Area is %f \n",x1,y1,x2,y2,x3,y3, area);
                
            choice = MenuPrint();
      }}
        return 0;
         
        }

int MenuPrint()

{

    int choice;

    printf("\nPlease choice a option:\n1> points closest together\n2> points further apart\n3>triangle with the smallest area\n4>triangle with the largest area\n5>exit\n");

    scanf_s("%d", &choice); 

    return choice;

}

double Distance(double x1, double y1, double x2, double y2)

{

    double distance;

    distance = sqrt(pow((x1-x2),2)+pow((y1-y2),2));

    return distance;

}

double LineLength (double pointsarray[][2], int number_of_points, int largest_or_smallest_line, double point1[][2], double point2[][2])
{

          if (largest_or_smallest_line == 1)

          {

                double x1, y1, x2, y2;

                double dist, dist_Min;

                int i, j;                

               int coord_1_min = 0; 

               int coord_2_min = 1;                

                x1 = pointsarray[coord_1_min][0]; //Getting the first element in the array pointsarray

                y1 = pointsarray[coord_1_min][1];

                x2 = pointsarray[coord_2_min][0];//Getting the second elemnt in the array pointsarray

                y2 = pointsarray[coord_2_min][1]; 

                dist_Min = Distance( x1, y1, x2, y2); // This first and the second points will get the initial minimum value

                for (i = 0; i < 19; i++) // The first point will run through the first element but not go till the last element. 
                { 
                   for (j = i + 1; j < 20; j++) //The scond point will run through the full list

                    {
                        x1 = pointsarray[i][0];

                        y1 = pointsarray[i][1];

                        x2 = pointsarray[j][0];

                        y2 = pointsarray[j][1];

                        dist = Distance( x1, y1, x2, y2);

                        if (dist < dist_Min) // if the calculated minimum is more than the initial minimum it will be replaced

                        {

                            coord_1_min = i;  // The address of the minimum element is put into coord_1_min

                            coord_2_min = j;   // The address of the minimum element is put into coord_2_min

                            dist_Min = dist; // Thenew distance is the minimum

                        }

                    }

                }

                         x1 = pointsarray[coord_1_min][0] ; // The value in point array is put into x1

                         y1 = pointsarray[coord_1_min][1] ;

                         x2 = pointsarray[coord_2_min][0] ;

                         y2 = pointsarray[coord_2_min][1] ; 

                         
                         point1[0][0] = x1; // The x1 is being placed in the array point1 so that they can be retuned 

                         point1[0][1] = y1;

                         point2[0][0] = x2;

                         point2[0][1] = y2;               
                          
                         return dist_Min;        // The minimum distanced is returned

          }   
          
          else if  (largest_or_smallest_line == 2) // Same as the miminum length except it is for the maximum vale

          {
            
                double x1, y1, x2, y2;

                double dist, dist_max;

                int i,j;                

               int coord_1_max = 0; 

               int coord_2_max = 1;                

                x1 = pointsarray[coord_1_max][0];

                y1= pointsarray[coord_1_max][1];

                x2 = pointsarray[coord_2_max][0];

                y2 = pointsarray[coord_2_max][1]; 

                dist_max = Distance( x1, y1, x2, y2); 

                for (i = 0; i < 19; i++)
                { 
                   for (j = i + 1; j < 20; j++)

                    {
                        x1 = pointsarray[i][0];

                        y1 = pointsarray[i][1];

                        x2 = pointsarray[j][0];

                        y2 = pointsarray[j][1];

                        dist = Distance( x1, y1, x2, y2);

                        if (dist > dist_max)

                        {

                            coord_1_max = i;

                            coord_2_max = j;

                            dist_max = dist;

                        }

                    }

                }

                         x1 = pointsarray[coord_1_max][0] ; 

                         y1 = pointsarray[coord_1_max][1] ;

                         x2 = pointsarray[coord_2_max][0] ;

                         y2 = pointsarray[coord_2_max][1] ;

                         
                          point1[0][0] = x1;

                         point1[0][1] = y1;

                         point2[0][0] = x2;

                         point2[0][1] = y2;  
                         
                         double line_length = dist_max;
                         
                         return line_length;                           

             }
}

double Area(double distance1, double distance2, double distance3 )

{

    double area, s;

    s = (distance1+distance2+distance3)/2;

    area = sqrt(s*(s-distance1)*(s-distance2)*(s-distance3));

    return area;

}

double AreaDifferenciator (double pointsarray[][2], int number_of_points, int largest_or_smallest_area, double vertex1[][2], double vertex2[][2],double vertex3[][2] )

{
   if (largest_or_smallest_area == 3)
       {
                double x1,y1,x2,y2,x3,y3;

                double dist_1_min, dist_2_min, dist_3_min, area_min;

                double dist_1, dist_2, dist_3, area;

                int i,j,k;                

                int iMin, jMin, kMin;

                int coord_1_min = 0; 

                int coord_2_min = 1;

                int coord_3_min = 2;

                iMin = 0;

                jMin = iMin + 1;

                kMin = jMin + 1;

                x1 = pointsarray[iMin][0];

                y1= pointsarray[iMin][1];

                x2 = pointsarray[jMin][0];

                y2 = pointsarray[jMin][1]; 

                x3 = pointsarray[kMin][0];

                y3 = pointsarray[kMin][1];

                dist_1_min = Distance (x1,y1,x2,y2);

                dist_2_min = Distance (x2,y2,x3,y3);

                dist_3_min = Distance (x3,y3,x1,y1);

                area_min = Area(dist_1_min, dist_2_min, dist_3_min);  // The first three pair points are used to intiial initial minimum area

                
                for (i = 0; i < 18; i++) //The program has run the whole number of points except the last second and last pair of points
                { 
                   for (j = i + 1; j < 19; j++)  // The program has run the whole number of points except the last pair of points
                    {                        
                        for (k = j + 1; k < 20; k++)  // The program has run the whole number of pair of points

                        {

                                    x1 = pointsarray[i][0];

                                    y1 = pointsarray[i][1];

                                    x2 = pointsarray[j][0];

                                    y2 = pointsarray[j][1];

                                    x3 = pointsarray[k][0];

                                    y3 = pointsarray[k][1];

                                    dist_1 = Distance (x1,y1,x2,y2);

                                    dist_2 = Distance (x2,y2,x3,y3);

                                    dist_3 = Distance (x3,y3,x1,y1);

                                    area = Area(dist_1, dist_2, dist_3);

                                    if (area < area_min)  // if the new calculated the min is less than the initla min

                                    {

                                        coord_1_min = i;  //then store their address in coord_1_min and same for next two steps

                                        coord_2_min = j;

                                        coord_3_min = k;

                                        area_min = area; // put the new area into the minimum area

                                    }

                                }

                            }

                          }

                          x1 = pointsarray[coord_1_min][0] ; 

                          y1 = pointsarray[coord_1_min][1];

                          x2 = pointsarray[coord_2_min][0];

                          y2 = pointsarray[coord_2_min][1]; 

                          x3 = pointsarray[coord_3_min][0];

                          y3 = pointsarray[coord_3_min][1];
                         
                         vertex1[0][0] = x1;

                         vertex1[0][1] = y1;

                         vertex2[0][0] = x2;

                         vertex2[0][1] = y2;
                         
                         vertex3[0][0] = x3; 
                         
                         vertex3[0][1] = y3;                
                         
                         return area_min;
         }

    else if (largest_or_smallest_area == 4)  // same as the min area except it finds the max area
    {
            double x1,y1,x2,y2,x3,y3;

                double dist_1_max, dist_2_max, dist_3_max, area_max;

                double dist_1, dist_2, dist_3, area;

                int i,j,k;                

                int imax, jmax, kmax;

                int coord_1_max = 0; 

                int coord_2_max = 1;

                int coord_3_max = 2;

                imax = 0;

                jmax = imax + 1;

                kmax= jmax + 1;

                x1 = pointsarray[imax][0];

                y1= pointsarray[imax][1];

                x2 = pointsarray[jmax][0];

                y2 = pointsarray[jmax][1]; 

                x3 = pointsarray[kmax][0];

                y3 = pointsarray[kmax][1];

                dist_1_max = Distance (x1,y1,x2,y2);

                dist_2_max = Distance (x2,y2,x3,y3);

                dist_3_max = Distance (x3,y3,x1,y1);

                area_max = Area(dist_1_max, dist_2_max, dist_3_max);
                
                for (i = 0; i < 18; i++)
                { 
                   for (j = i + 1; j < 19; j++)
                    {                        
                        for (k = j + 1; k < 20; k++)

                        {

                                    x1 = pointsarray[i][0];

                                    y1 = pointsarray[i][1];

                                    x2 = pointsarray[j][0];

                                    y2 = pointsarray[j][1];

                                    x3 = pointsarray[k][0];

                                    y2 = pointsarray[k][1];

                                    dist_1 = Distance (x1,y1,x2,y2);

                                    dist_2 = Distance (x2,y2,x3,y3);

                                    dist_3 = Distance (x3,y3,x1,y1);

                                    area = Area(dist_1, dist_2, dist_3);

                                    if (area > area_max)

                                    {

                                        coord_1_max = i;

                                        coord_2_max = j;

                                        coord_3_max = k;

                                        area_max= area;

                                    }

                                }

                            }

                          }

                          x1 = pointsarray[coord_1_max][0] ; 

                          y1 = pointsarray[coord_1_max][1];

                          x2 = pointsarray[coord_2_max][0];

                          y2 = pointsarray[coord_2_max][1]; 

                          x3 = pointsarray[coord_3_max][0];

                          y3 = pointsarray[coord_3_max][1];
                         
                         vertex1[0][0] = x1;

                         vertex1[0][1] = y1;

                         vertex2[0][0] = x2;

                         vertex2[0][1] = y2;
                         
                         vertex3[0][0] = x3; 
                         
                         vertex3[0][1] = y3;                
                         
                         return area_max;
    }
}


 