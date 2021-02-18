#include <time.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <signal.h>
#include <errno.h>

int main(int argc, char **argv) {
#define cmdAmount 100//max chars a command string can hold

char* prompt = "308sh> ";
char command[cmdAmount];

if(argc != 1 && argc != 3)
{
	printf("error invalid args");
	return 0;}
else if(argc==3){
	if(strcmp(argv[1], "-p")!=0){
		printf("error");
		return 0;
	}else{
		prompt = argv[2];
	}
}

while(1){//user input infinite loop

	printf(prompt);//print prompt = "308sh" for users;

	
	fgets(command, cmdAmount, stdin);//retrieves user input
	
	int cmdSize = strlen(command);
	if(command[cmdSize-1]=='/n' && cmdSize>0){
		command[cmdSize-1] = '\0';//last char to a null
	}
       
		//if (fork() !=0){
		//	wait(NULL);
		//}else{

//---------------built in commands-------------------------------------------------

//print statement for decoding
//printf("command:%s", command);

	//check if a background process is cureently running or finished
	int childStatus;
	int waitID=1;
	//looks for processes that executed
	while(waitID > 0){
		waitID = waitpid(-1, &childStatus, WNOHANG);

		if(waitID > 0){//prints the status info
			printf("Exit status of child: %d\n", WEXITSTATUS(childStatus));
		}
	}
	
	if(strcmp(command,"exit\n") ==0){
		return 0;}//exit if the user types exit

	else if(strlen(command)<1){//checks if user failed to eneter a cmd
	printf("\n");
}
	
	else if (strcmp(command, "pid\n")==0){//looks if user wants a pid cmd to see the shell process's id
		printf("Shell Process ID: %d\n",getpid());}
	
	else if (strcmp(command, "ppid\n")==0){
		printf("Shell Process's Parent ID: %d\n",getppid());}//looks if user wants a pid cmd to see the shell process parent's id
	
	else if (strncmp(command, "cd", 2)==0){//looks if user wants to change directories

		if(strlen(command)==2){
			chdir(getenv("HOME"));
		}	
		else{
			char cdDiry[strlen(command)-3];
			strcpy(cdDiry, command+3);

			if(chdir(cdDiry)== -1){
				perror("error");//prints out the error
			}
		}
	}

	else if (strcmp(command, "pwd\n")==0){//checks if user wants its current working directoy printed

		char workingDiry[cmdAmount];
		getcwd(workingDiry, cmdAmount);

		if (getcwd(workingDiry, cmdAmount)==NULL){
			perror("error");//prints out the error
		}

		printf("%s\n",workingDiry);
	}

	else{//lets the user know if the command was unrecognizable
		printf("command not found\n");}

		//}//end of else fork
	
}//end of while(1)

return 0;

}//end of main
