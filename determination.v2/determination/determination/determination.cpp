#include "pch.h"
#include <iostream>
#include "CDeterminator.h"

using namespace std;

int main(int argc, char* argv[])
{
	CDeterminator determinator;
	ofstream output("output.txt");
	determinator.determineAutomat(argv[1]);
	determinator.showRefactDeterministicAutomat(output);
	determinator.createDotFile();
}