#include "Bank.h"
#include "Bank.c"
#include <time.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <signal.h>
#include <errno.h>
#include <pthread.h>

struct Trans {     // structure for a transaction pair
	int acc_id;// account ID
	int amount;// amount to be added, could be positive or negative
};

struct Req {
	struct request * next; // pointer to the next request in the list
	int request_id;// request ID assigned by the main thread
	int check_acc_id; // account ID for a CHECK request
	struct trans * transactions; // array of transaction data
	int num_trans;// number of accounts in this transaction
	struct timeval starttime, endtime; // starttime and endtime for TIME
};

struct Que {
	struct request * head, * tail; // head and tail of the list
	int num_jobs;// number of jobs currently in queue
};




pthread_mutex_t mutex;
pthread_mutex_t queue_mut;
pthread_cond_t og;
pthread_cond_t work;

int main (int argc, char *argv[])
{ 
	#define cmdAmount 100//max chars a command string can hold
	char* prompt = "Project2-shell> ";
	char command[cmdAmount];

	int workerThreads;
	int numberOfAccounts;
	FILE *fp = NULL;

	if(atoi(argv[1])==0){
		printf("An error occured while opening the number of accounts, try again.\n");
		return 0;
	}else{
		workerThreads = atoi(argv[1]);
	}
	
	if(atoi(argv[2])==0){
		printf("An error occured while opening the number of accounts, try again.\n");
		return 0;
	}else{
		numberOfAccounts = atoi(argv[2]);
		//number of bank accounts to initialize, passed to serv as command line
		//void initialize_accounts(numberOfAccounts);
	}

	fp = fopen(argv[3],"w");
	if(fp==NULL){
		printf("An error occured while opening the specificed file, try again.\n");
		return 0;
	}else{
		printf("Worker threads: %d\n", workerThreads);
		printf("Number of accounts: %d\n", numberOfAccounts);
		printf("Output file: %s\n", argv[3]);
	}

	//initalize bank accounts and mutex lists
	pthread_t the_main;
	pthread_t the_workers[workerThreads];

	pthread_mutex_init(&mutex, NULL);    
	pthread_mutex_init(&queue_mut,NULL);
	//pthread_mutex_cond(&work,NULL);

	//pthread_create(&workedLoop, NULL);//thread cration


	while(1){//user input infinite loop
		printf(prompt);//print prompt = "308sh" for users;
	
		fgets(command, cmdAmount, stdin);//retrieves user input

		if(strlen(command)<1){
			printf("user failed to enter a cmd\n");
		}

		int count = 0;//set count
		char * pch;
		pch = strtok(command," ");
		while(pch !=NULL){
			count++;
			pch = strtok(NULL," ");
	
		}

		char commandInput[count+1][100];
		//[number string][size of string]
		//printf("amount of inputs:%d\n",count);

		char * pch2;
		pch2 = strtok(command," ");
		int count2 = 0;
		while(pch2 !=NULL){
			strcpy(commandInput[count2],pch2);
			pch2 = strtok(NULL," ");
			
			count2++;
		}
//printf("%s is the 1st param(test)\n", commandInput[0]);
		
		

		if (strlen(command)<1){ // If no args were entered, give back error
			printf("Error: no args were entered\nType 'END' to end program\n");
		}
		else if(strcmp(commandInput[0],"END\n") ==0)
		{
			return 0;
		}
		else if (strcmp(commandInput[0],"CHECK")==0) // Request is a CHECK
		{
			//Check if valid request
			// Valid request args
			// Create Request
			// Make sure it is not a duplicate
			// Add request to queue
		}
		else if (strcmp(commandInput[0],"TRANS")==0) // Request is a transaction
		{
			//should be odd
			//socant be even amount of args
			if (count%2==0) //Invalid number of args in transaction
			{
				printf("invalid number of args in transaction\n");
				printf("Ex: TRANS <acct1> <amount1> <acct2> <amount2> ...\n");
				return 0;
			}
			//Correct number of args. Create Request

			// Make sure no duplicate accounts in request
			int i;
			int j;
			for (i=1;i<count;i=i+2)
			{
				for (j=1;j<count;j=j+2)
				{
					if(i!=j)
					{
						printf("i: %c\n",commandInput[i]);
						printf("j: %c\n",commandInput[j]);

						if(commandInput[i]==commandInput[j])
						{
							printf("i: %c\n",commandInput[i]);
							printf("j: %c\n",commandInput[j]);
							printf("duplicate accounts in request\n");
							return 0;
						}
					}	
				
				}
			}

			// Make sure both acc_id and amount are ints and in range
			int q;
			for (q=1;q<count;q++)
			{
				if(atoi(commandInput[q])>9999)
				{
					printf("%c\n", atoi(commandInput[1]));
printf("%d\n", atoi(commandInput[2]));
printf("%c\n", commandInput[3]);
printf("%c\n", atoi(commandInput[4]));
					printf("acc_id or amount ints are not in range\n");
					return 0;
				}
			}

			// Make sure request is not a duplicate
			// Invalid transaction request,
			//Add request to queue
		}
		else
		{ //Give input error message
			printf("input error message\n");
			return 0;
		}
	}




}
	/*
	pthread_t t1;//thread id!
	pthread_t t2;	
	
	pthread_create(&t1, NULL,thread1, NULL);//thread creations!
	pthread_create(&t2, NULL,thread2, NULL);

	pthread_join(t1, NULL);//makes main wait for open threads
	pthread_join(t2, NULL);
	*/


/*
//This method adds a request to the queue.

void addReqToQueue(Request *reqHold)
{
}


//This method returns a request from the queue of request.

Request *getReqFromQueue()
{
}
*/

void *workerLoop(void *);

void* workedLoop(void * arg)
{   
	/*worker loop, pull from the queue. keeps pulling requests until the server is shutting down*/
	while(1)
	{
		if(1==1)//it is a check request
		{
			//get the balance of an account
			//write the balance to output file
		}
		else
		{//it is a transaction request
			//get a lock on the accounts
			//check that all are good to go, give back the bad account if there is one
			//do the transaction if all are good
			//unlock all accounts
			//trans is good,output specfific text
		}
	}
}

void* thread2(void * arg)
{ 
	sleep(5);//finishes before main w/out join
	printf("Hello from thread2\n");
}
