#include "pch.h"
#include "Minimizator.h"



Minimizator::Minimizator(std::string const& fileName)
{
	minimilizeAutomat(fileName);
}

void Minimizator::minimilizeAutomat(std::string fileName) {
	std::ifstream inputFile(fileName);
	if (!inputFile.is_open()) {
		std::cout << "File cannot be opened!\n";
	}
	else {
		std::string typeOfAutomat;
		std::string strAmountOFInputSignals;
		getline(inputFile, typeOfAutomat);
		getline(inputFile, strAmountOFInputSignals);
		int numberType = stoi(typeOfAutomat);
		m_amountInputSignlas = stoi(strAmountOFInputSignals);
		if (numberType == 2) {
			readOriginalAutomatMiliFromFile(inputFile);
		}
		else {
			readOriginalAutomatMurFromFile(inputFile);
		}
		startMinimization();
	}
}

std::vector<int> getVectorOfActions(std::stringstream &streamStr) {
	std::string str;
	std::vector<int> vectorOfActions;
	std::string numberStr;
	while (streamStr >> numberStr) {
		vectorOfActions.push_back(stoi(numberStr));
	}
	return vectorOfActions;
}

void Minimizator::readOriginalAutomatMurFromFile(std::ifstream &inputFile) {
	std::string str;
	getline(inputFile, str);
	getline(inputFile, str);
	size_t amountState = stoi(str);
	getline(inputFile, str);
	AutomatState state;
	std::stringstream streamStr;
	streamStr << str;
	std::vector<int> vectorOfActions = getVectorOfActions(streamStr);
	std::string numberStr;
	streamStr.clear();
	while (getline(inputFile, str)) {
		streamStr << str;
		std::vector<AutomatState> stateVector;
		for (size_t i = 0; i < amountState; i++) {
			state.action = vectorOfActions[i];
			streamStr >> numberStr;
			state.vertex = stoi(numberStr) - 1;
			stateVector.push_back(state);
		}
		streamStr.clear();
		m_originalAutomat.push_back(stateVector);
	}

}

void Minimizator::readOriginalAutomatMiliFromFile(std::ifstream &inputFile) {
	std::string str;
	getline(inputFile, str);
	getline(inputFile, str);
	AutomatState state;
	while (getline(inputFile, str)) {
		std::stringstream streamStr;
		streamStr << str;
		std::string numberStr;
		std::vector<AutomatState> stateVector;
		while (streamStr >> numberStr) {
			state = getStateFromStr(numberStr);
			stateVector.push_back(state);
		}
		m_originalAutomat.push_back(stateVector);
	}
}

AutomatState Minimizator::getStateFromStr(const std::string &str) {
	std::string numberStr = "";
	AutomatState state;
	size_t i = 0;
	while (str[i] != '/' && i < str.size()) {
		numberStr += str[i];
		i++;
	}
	i++;
	state.vertex = stoi(numberStr) - 1;
	numberStr = "";
	while (i < str.size()) {
		numberStr += str[i];
		i++;
	}
	state.action = stoi(numberStr);
	return state;
}

void Minimizator::startMinimization() {
	std::vector<EquivalentClasseStruct> vectorOfEquivClasses = determineEquivalentClasses();
	//showEquivalentClasses(vectorOfEquivClasses);
	//std::cout << std::endl;
	int amountClasses = 0;
	while (amountClasses != vectorOfEquivClasses.size()) {
		amountClasses = vectorOfEquivClasses.size();
		std::vector<std::vector<Section>> vect = createMinorTable(vectorOfEquivClasses);
		vectorOfEquivClasses = getVectorOfEquivalentClasses(vect, vectorOfEquivClasses);
		//showNewTable(vect);
		//showEquivalentClasses(vectorOfEquivClasses);
		//std::cout << std::endl;
	}
	m_miniAutomat = createMinimilizeAutomat(vectorOfEquivClasses);
}

std::vector<EquivalentClasseStruct> Minimizator::determineEquivalentClasses() {
	std::vector<EquivalentClasseStruct> vectorOfEquivalentClass;
	for (size_t i = 0; i < m_originalAutomat[0].size(); i++) {
		EquivalentClasseStruct equivalentClass;
		for (size_t j = 0; j < m_originalAutomat.size(); j++) {
			equivalentClass.actions.push_back(m_originalAutomat[j][i].action);
		}
		equivalentClass.vertexs.push_back(i);
		equivalentClass.id = vectorOfEquivalentClass.size();
		int pos = findSimilarEquivalentClass(equivalentClass, vectorOfEquivalentClass);
		if (pos == -1) {
			vectorOfEquivalentClass.push_back(equivalentClass);
		}
		else {
			vectorOfEquivalentClass[pos].vertexs.push_back(i);
		}
	}
	return vectorOfEquivalentClass;
}

int Minimizator::findSimilarEquivalentClass(EquivalentClasseStruct &equivClass, std::vector<EquivalentClasseStruct> &vector) {
	for (size_t i = 0; i < vector.size(); i++) {
		if (equivClass.actions == vector[i].actions) {
			return i;
		}
	}
	return -1;
}

std::vector<std::vector<Section>> Minimizator::createMinorTable(std::vector<EquivalentClasseStruct> &vect) {
	std::vector<std::vector<Section>> newTable(m_amountInputSignlas);
	for (size_t i = 0; i < vect.size(); i++) {
		for (size_t j = 0; j < newTable.size(); j++) {
			for (size_t k = 0; k < vect[i].vertexs.size(); k++) {
				Section s;
				s.vertex = vect[i].vertexs[k];
				s.id = getCorrespondingClass(s.vertex, j, vect);
				newTable[j].push_back(s);
			}
		}
	}
	return newTable;
}

int Minimizator::getCorrespondingClass(int vertex, int signal, std::vector<EquivalentClasseStruct> &vect) {
	int vert = m_originalAutomat[signal][vertex].vertex;
	for (size_t i = 0; i < vect.size(); i++) {
		if (find(vect[i].vertexs.begin(), vect[i].vertexs.end(), vert) != vect[i].vertexs.end()) {
			return vect[i].id;
		}
	}
	return -1;
}

int findEquivalentClasses(int vertex, std::vector<EquivalentClasseStruct> &vector) {
	for (size_t i = 0; i < vector.size(); i++) {
		if (find(vector[i].vertexs.begin(), vector[i].vertexs.end(), vertex) != vector[i].vertexs.end()) {
			return vector[i].id;
		}
	}
	return 0;
}

int Minimizator::findSimilarEquivalentClass(EquivalentClasseStruct &equivClass, std::vector<EquivalentClasseStruct> &vector, std::vector<EquivalentClasseStruct> &oldVector) {

	for (size_t i = 0; i < vector.size(); i++) {
		if (equivClass.actions == vector[i].actions && findEquivalentClasses(equivClass.vertexs[0], oldVector) == findEquivalentClasses(vector[i].vertexs[0], oldVector)) {
			return i;
		}
	}
	return -1;
}

std::vector<EquivalentClasseStruct> Minimizator::getVectorOfEquivalentClasses(std::vector<std::vector<Section>> &vect, std::vector<EquivalentClasseStruct> oldVector) {
	std::vector<EquivalentClasseStruct> vectorOfEquivalentClass;
	for (size_t i = 0; i < vect[0].size(); i++) {
		EquivalentClasseStruct equivalentClass;
		for (size_t j = 0; j < vect.size(); j++) {
			equivalentClass.actions.push_back(vect[j][i].id);
		}
		equivalentClass.vertexs.push_back(vect[0][i].vertex);
		equivalentClass.id = vectorOfEquivalentClass.size();
		int pos = findSimilarEquivalentClass(equivalentClass, vectorOfEquivalentClass, oldVector);
		if (pos == -1) {
			vectorOfEquivalentClass.push_back(equivalentClass);
		}
		else {
			vectorOfEquivalentClass[pos].vertexs.push_back(vect[0][i].vertex);
		}
	}
	return vectorOfEquivalentClass;
}

std::vector<std::vector<AutomatState>> Minimizator::createMinimilizeAutomat(std::vector<EquivalentClasseStruct> &vect) {
	std::vector<std::vector<AutomatState>> minimilizeAutomat(m_amountInputSignlas);
	for (size_t i = 0; i < minimilizeAutomat.size(); i++) {
		for (size_t j = 0; j < vect.size(); j++) {
			AutomatState state;
			state.vertex = m_originalAutomat[i][vect[j].vertexs[0]].vertex;
			state.vertex = getVertex(vect, state.vertex);
			state.action = m_originalAutomat[i][vect[j].vertexs[0]].action;
			minimilizeAutomat[i].push_back(state);
		}
	}
	return minimilizeAutomat;
}

int Minimizator::getVertex(std::vector<EquivalentClasseStruct> &vect, int vertex) {
	for (size_t i = 0; i < vect.size(); i++) {
		if (find(vect[i].vertexs.begin(), vect[i].vertexs.end(), vertex) != vect[i].vertexs.end()) {
			return i;
		}
	}
	return 0;
}

void Minimizator::showOriginalAutomat() {
	for (size_t i = 0; i < m_originalAutomat.size(); i++) {
		for (size_t j = 0; j < m_originalAutomat[i].size(); j++) {
			std::cout << m_originalAutomat[i][j].vertex + 1 << '/' << m_originalAutomat[i][j].action << ' ';
		}
		std::cout << std::endl;
	}
}

void Minimizator::showMinimilizeAutomat() {
	for (size_t i = 0; i < m_miniAutomat.size(); i++) {
		for (size_t j = 0; j < m_miniAutomat[i].size(); j++) {
			std::cout << m_miniAutomat[i][j].vertex << '/' << m_miniAutomat[i][j].action << ' ';
		}
		std::cout << std::endl;
	}
}

void Minimizator::showEquivalentClasses(std::vector<EquivalentClasseStruct> &vector) {
	for (size_t i = 0; i < vector.size(); i++) {
		std::cout << vector[i].id + 1 << ": ";
		for (size_t j = 0; j < vector[i].vertexs.size(); j++) {
			std::cout << vector[i].vertexs[j] + 1 << ' ';
		}
		std::cout << std::endl;
	}
}

void Minimizator::showNewTable(std::vector<std::vector<Section>> &vect) {
	for (size_t i = 0; i < vect.size(); i++) {
		for (size_t j = 0; j < vect[i].size(); j++) {
			std::cout << vect[i][j].id + 1 << '/' << vect[i][j].vertex + 1 << ' ';
		}
		std::cout << std::endl;
	}
}

Minimizator::~Minimizator()
{
}
