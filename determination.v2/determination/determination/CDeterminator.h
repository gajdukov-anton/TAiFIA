#pragma once
#include <vector>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
#include <algorithm>

struct Alphabet
{
	int value;
	std::vector<int> vertex;
};

class CDeterminator
{
public:
	CDeterminator();

	void determineAutomat(std::string fileName);
	void showNonDeterministicAutomat();
	void showFinalStates();
	void showDeterministicAutomat();
	void showRefactDeterministicAutomat(std::ofstream& output);
	void createDotFile();

	~CDeterminator();
private:
	int m_xAmount;
	int m_qAmount;
	int m_FinalStateAmount;
	std::vector<int> m_finalStatesOfNonDetermAutomat;
	std::vector<int> m_finalStatesOfDetermAutomat;
	std::vector<std::vector<std::vector<int>>> m_tableTransitionsNonDetermAutomat;
	std::vector<std::vector<std::vector<int>>> m_tableTransitionsDetermAutomat;
	std::vector<std::vector<int>> m_vertexTranstionDetermAutomat;
	std::vector<Alphabet> m_alphabet;
	std::vector<std::vector<int>> m_determAutomat;

	void readDataFromFile(std::string& fileName);
	void readFinalStates(std::ifstream& inputFile);
	void readNonDeterministicAutomat(std::ifstream& inputFile);
	void createDetermTransition();
	std::vector<int> createNewState(std::vector<int>& vertex, int signal);
	void fillAlphabet();
	void fillDetermAutomat();
	void fillFinalStatesOfDetermAutomat();
};

