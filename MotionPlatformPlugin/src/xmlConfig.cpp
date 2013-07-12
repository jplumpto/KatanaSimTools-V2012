#include "../inc/xmlConfig.h"


XmlConfig::XmlConfig()
{
	strcpy(HostIP , "192.168.0.81");
	strcpy(Port, "5345");
	
}

void XmlConfig::Open(const char *filename)
{
	char line[1000];
	char *entry;

	//Open the config file
	FILE *fp = fopen(filename,"r");

	//If not opened, return a NULL
	if (!fp)
	{
		IsOpen = 0;
		return;
	}



	//Loop through XML
	while (!feof(fp))
	{
		//Readline
		fgets(line,sizeof(line),fp);

		//Determine node
		entry = strtok(line,"<>");
		entry = strtok(NULL,"<>");

		if (!entry)
		{
			break;
		}

		//Now determine what it contains
		if (strcmp(entry,"HostIP") == 0)
		{
			entry = strtok(NULL,"<>");
			strcpy(HostIP,entry);
		} 
		else if (strcmp(entry,"Port") == 0)
		{
			entry = strtok(NULL,"<>");
			strcpy(Port,entry);
		}
	}

	IsOpen = 1;
}