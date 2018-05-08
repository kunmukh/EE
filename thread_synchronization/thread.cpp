//File: thread.cpp                                                               
//THis project is to implement a senario where there may be multiple 
//producers and consumers accessing a bounded buffer as a circular 
//queue, each of which is running in a thread.      
//-----------------------------------------------------------------             
// Class: CS 470                     Instructor: Dr. Deborah Hwang              
// Assignment:    Thread Synchronization Project 
// Date assigned:  1/24/2018                  
// Programmer:    Kunal Mukherjee    Date completed: 2/7/2018 

#include "buffer.h"
#include <cstdlib>
#include <iostream>
#include <unistd.h>
#include <time.h>
#include <semaphore.h>

int sleep_time; //the sleep_time shared between the processes
//declaring semaphores and pthread_mutex
sem_t empty;
sem_t full;
pthread_mutex_t mutex;
pthread_mutex_t mutex_cout;

#define MAX_ITEM 5000

using namespace std;

void *Producer (void* param);
void *Consumer (void* param);

//main gets the command line argument
//initialize buffer
//creates producer threads
//creates consumer threads
//sleep
//exit
int main(int argc, char* argv[]){

	//check to see if all the arguments are corrrectly entered
	if (argc != 4){
		cout << "usage: " << argv[0] << " sleep_time num_producer_thread num_comsumer_thread " << endl;
		return 0;
	}
	//storing variable from input
	int num_producer_thread;
	int num_consumer_thread;

	//variables intitialized
	sleep_time = atoi(argv[1]);
	num_producer_thread = atoi(argv[2]);
	num_consumer_thread = atoi(argv[3]);

	//semaphores intiialized
	sem_init(&empty, 0, BUFFER_SIZE);
	sem_init(&full, 0, 0);

	//inititalize the buffer
	for (int i = 0; i < BUFFER_SIZE; i++){
		buffer[i] = -1;
	}

	//initialize random number generator
	srand(time(0));

	//creating pthread arrays for producer and consumer
	pthread_t producer_thread [100];
	pthread_t consumer_thread [100];

	//creating the producer threads
	for (int i = 0; i < num_producer_thread; i++){

		if (pthread_create(&producer_thread[i], NULL, Producer, (void *)(intptr_t) i)){
			cout << "Producer thread cannot be created" << endl;
			return 1;
		}
		else
			cout << "Created Producer # " << i << endl;
	}

	//creating the consumer threads
	for (int i = 0; i < num_consumer_thread; i++){

		if (pthread_create(&consumer_thread[i], NULL, Consumer, (void *)(intptr_t) i)){
			cout << "consumer thread cannot be created" << endl;
			return 1;
		}
		else
			cout << "Created consumer # " << i << endl;
	}

	printBuffer();
	
	sleep(sleep_time);

	return 0;
	
	//return 0;

	
}

void *Producer (void* param){
	buffer_item item;

	int pid = (intptr_t)param;

	do{
	  //sleep at a random time in between the main sleeps
	  sleep (rand() % (sleep_time + 1));

	  //get a random number
	  item = rand() % (MAX_ITEM + 1);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Producer # " << pid << " waiting on empty semaphore" << endl;

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);

	  //wait for empty semaphore
	  sem_wait(&empty);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Producer # " << pid << " getting mutex lock" << endl;

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);

	  //getting mutex lock for start entering items
	  pthread_mutex_lock(&mutex);

	  insert_item(item);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);	  

	  cout << "Producer # " << pid << " produced " << item << endl;

	  printBuffer();

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Producer # " << pid << " releasing mutex lock" << endl;	  

	  //set cout mutex unlock	
	  pthread_mutex_unlock(&mutex_cout);

	  pthread_mutex_unlock(&mutex);	

	  cout << "Producer # " << pid << " signalling full" << endl;

	  cout << "\n\n\n\n" << endl;

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);			

	  //unlock full
	  sem_post (&full);	 	

	}while(1);
}

void *Consumer (void* param){
	buffer_item item;

	int pid = (intptr_t)param;

	do{	  

	  //sleep at a random time in between the main sleeps
	  sleep (rand() % (sleep_time));	

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Consumer # " << pid << " waiting on full semaphore" << endl;

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);

	  //wait for full semaphore
	  sem_wait(&full);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Consumer # " << pid << " getting mutex lock" << endl;		

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);		

	  //getting mutex lock for start entering items
	  pthread_mutex_lock(&mutex);

	  remove_item(&item);
		 
	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Consumer # " << pid << " consumed " << item << endl;

	  printBuffer();

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Consumer # " << pid << " releasing mutex lock" << endl;	  

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);	  

	  //releasing mutex lock
	  pthread_mutex_unlock(&mutex);

	  //get cout mutex lock	
	  pthread_mutex_lock(&mutex_cout);

	  cout << "Consumer # " << pid << " signalling empty" << endl;

	  cout << "\n\n\n\n" << endl;

	  //get cout mutex lock	
	  pthread_mutex_unlock(&mutex_cout);	  

	  //unlock the semaphore refrence
	  sem_post (&empty); 	
		
	}while(1);
}
