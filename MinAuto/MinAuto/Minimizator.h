#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include <sstream>

struct AutomatState {
	int vertex = 0;
	int action = 0;
};

struct EquivalentClasseStruct {
	int id = 0;
	std::vector<int> vertexs;
	std::vector<int> actions;

};

struct Section {
	int id;
	int vertex;
};

class Minimizator
{
public:
	Minimizator(std::string const& fileName);
	void showMinimilizeAutomat();
	void showOriginalAutomat();
	~Minimizator();
private:
	int m_amountInputSignlas;
	std::vector<std::vector<AutomatState>> m_originalAutomat;
	std::vector<std::vector<AutomatState>> m_miniAutomat;
	void startMinimization();
	void minimilizeAutomat(std::string fileName);
	void showEquivalentClasses(std::vector<EquivalentClasseStruct> &vector);
	void showNewTable(std::vector<std::vector<Section>> &vect);
	int getVertex(std::vector<EquivalentClasseStruct> &vect, int vertex);
	void readOriginalAutomatMiliFromFile(std::ifstream &inputFile);
	void readOriginalAutomatMurFromFile(std::ifstream &inputFile);
	AutomatState getStateFromStr(const std::string &str);
	std::vector<EquivalentClasseStruct> determineEquivalentClasses();
	int findSimilarEquivalentClass(EquivalentClasseStruct &equivClass, std::vector<EquivalentClasseStruct> &vector);
	int findSimilarEquivalentClass(EquivalentClasseStruct &equivClass, std::vector<EquivalentClasseStruct> &vector, std::vector<EquivalentClasseStruct> &oldVector);
	std::vector<std::vector<Section>> createMinorTable(std::vector<EquivalentClasseStruct> &vect);
	int getCorrespondingClass(int vertex, int signal, std::vector<EquivalentClasseStruct> &vect);
	std::vector<EquivalentClasseStruct> getVectorOfEquivalentClasses(std::vector<std::vector<Section>> &vect, std::vector<EquivalentClasseStruct> oldvector);
	std::vector<std::vector<AutomatState>> createMinimilizeAutomat(std::vector<EquivalentClasseStruct> &vect);
};

