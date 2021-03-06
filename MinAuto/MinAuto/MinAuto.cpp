// MinAuto.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include "pch.h"
#include "Minimizator.h"
#include <iostream>

int main(int argc, char* argv[])
{
	if (argc < 2 || argc > 3) {
		std::cout << "Enter name of file with automat." << std::endl;
		return 1;
	} 
	try {
		Minimizator minimizator(argv[1]);
		if (argc == 3) {
			std::ofstream outFile(argv[2]);
			minimizator.showMinimilizeAutomat(outFile);
		}
		else {
			minimizator.showMinimilizeAutomat(std::cout);
		}
		minimizator.createDotFile();
	}
	catch (std::invalid_argument e) {
		std::cout << e.what();
	}	
	return 0;
}