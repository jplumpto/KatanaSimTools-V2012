//---------------------------------------------------------------------
// XML Config header file
//---------------------------------------------------------------------
#ifndef _XML_CONFIG_H_
#define _XML_CONFIG_H_

// Disable depreciation warning.
#pragma warning( disable : 4996 )

#include <stdio.h> 
#include <conio.h> 
#include <string.h>
#include <ctype.h>
#include <stdlib.h>


class XmlConfig {
public:
	XmlConfig();                            // constructor; initialize the list to be empty
	void Open(const char *filename);         // Opens the specified Config file and returns the parameters

	char HostIP[100];
	char Port[100];

	int IsOpen;

private:
};



#endif
