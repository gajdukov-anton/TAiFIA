#include "pch.h"
#include "CDeterminator.h"


CDeterminator::CDeterminator()
{
}


void CDeterminator::determineAutomat(std::string fileName)
{
	readDataFromFile(fileName);
	createDetermTransition();
	fillAlphabet();
	fillDetermAutomat();
	fillFinalStatesOfDetermAutomat();
}

void CDeterminator::readDataFromFile(std::string& fileName)
{
	std::ifstream inputFile(fileName);
	std::string numberStr;
	getline(inputFile, numberStr);
	m_xAmount = stoi(numberStr);
	getline(inputFile, numberStr);
	m_qAmount = stoi(numberStr);
	getline(inputFile, numberStr);
	m_FinalStateAmount = stoi(numberStr);
	readFinalStates(inputFile);
	readNonDeterministicAutomat(inputFile);
}

void CDeterminator::readFinalStates(std::ifstream& fileName)
{
	std::string numberStr;
	std::stringstream streamStr;
	getline(fileName, numberStr);
	streamStr << numberStr;
	while (streamStr >> numberStr) {
		m_finalStatesOfNonDetermAutomat.push_back(stoi(numberStr));
	}
}

void CDeterminator::readNonDeterministicAutomat(std::ifstream& fileName)
{
	std::string numberStr;
	std::stringstream streamStr;
	m_tableTransitionsNonDetermAutomat.resize(m_qAmount);
	for (size_t i = 0; i < m_tableTransitionsNonDetermAutomat.size(); i++) {
		m_tableTransitionsNonDetermAutomat[i].resize(m_xAmount);
	}
	for (int i = 0; i < m_qAmount; i++) {
		getline(fileName, numberStr);
		streamStr << numberStr;
		while (streamStr >> numberStr) {
			int vertex = stoi(numberStr);
			streamStr >> numberStr;
			int signal = stoi(numberStr);
			m_tableTransitionsNonDetermAutomat[i][signal].push_back(vertex);
		}
		for (size_t j = 0; j < m_xAmount; j++) {
			std::sort(m_tableTransitionsNonDetermAutomat[i][j].begin(), m_tableTransitionsNonDetermAutomat[i][j].end());
		}
		streamStr.clear();
	}
}

void CDeterminator::createDetermTransition()
{
	std::vector<std::vector<int>> needToTreet;
	std::vector<int> startVertex;
	startVertex.push_back(0);
	m_tableTransitionsDetermAutomat.push_back(m_tableTransitionsNonDetermAutomat[0]);
	for (int i = 0; i < m_tableTransitionsDetermAutomat.size(); i++) {
		needToTreet.push_back(m_tableTransitionsDetermAutomat[0][i]);
	}
	m_vertexTranstionDetermAutomat.push_back(startVertex);
	for (int i = 0; i < needToTreet.size(); i++) {
		std::vector<std::vector<int>> transitions;
		for (int j = 0; j < m_xAmount; j++) {
			std::vector<int> state = createNewState(needToTreet[i], j);
			transitions.push_back(state);
			if (std::find(needToTreet.begin(), needToTreet.end(), state) == needToTreet.end() && state.size() > 0 ) {
				if (!(state.size() == 1 && state[0] == 0)) {
					needToTreet.push_back(state);
				}
			}

		}
		m_tableTransitionsDetermAutomat.push_back(transitions); 
		m_vertexTranstionDetermAutomat.push_back(needToTreet[i]);
	}
}

std::vector<int> CDeterminator::createNewState(std::vector<int>& vertex, int signal)
{
	std::vector<int> result;
	for (int i = 0; i < vertex.size(); i++) {
		for (int j = 0; j < m_tableTransitionsNonDetermAutomat[vertex[i]][signal].size(); j++) {
			if (std::find(result.begin(), result.end(), m_tableTransitionsNonDetermAutomat[vertex[i]][signal][j]) == result.end()) {

				result.push_back(m_tableTransitionsNonDetermAutomat[vertex[i]][signal][j]);
			}
		}
	}
	std::sort(result.begin(), result.end());
	return result;
}

void CDeterminator::fillAlphabet() 
{
	for (size_t i = 0; i < m_vertexTranstionDetermAutomat.size(); i++) {
		Alphabet state;
		state.value = i;
		state.vertex = m_vertexTranstionDetermAutomat[i];
		m_alphabet.push_back(state);
	}
}

int  getValueFromAlphabet(std::vector<Alphabet>& alphabet, std::vector<int>& key)
{
	for (size_t i = 0; i < alphabet.size(); i++) {
		if (alphabet[i].vertex == key) {
			return alphabet[i].value;
		}
	}
	return -1;
}

void CDeterminator::fillDetermAutomat()
{
	m_determAutomat.resize(m_tableTransitionsDetermAutomat.size());
	for (size_t i = 0; i < m_tableTransitionsDetermAutomat.size(); i++) {
		for (size_t j = 0; j < m_tableTransitionsDetermAutomat[i].size(); j++) {
			m_determAutomat[i].push_back(getValueFromAlphabet(m_alphabet, m_tableTransitionsDetermAutomat[i][j]));
		}
	}
}

void CDeterminator::fillFinalStatesOfDetermAutomat()
{
	std::vector<std::vector<int>> finalStatesBeforeTreatment;
	for (size_t i = 0; i < m_vertexTranstionDetermAutomat.size(); i++) {
		for (size_t j = 0; j < m_finalStatesOfNonDetermAutomat.size(); j++) {
			if (std::find(m_vertexTranstionDetermAutomat[i].begin(), m_vertexTranstionDetermAutomat[i].end(), m_finalStatesOfNonDetermAutomat[j]) != m_vertexTranstionDetermAutomat[i].end()
				&& std::find(finalStatesBeforeTreatment.begin(), finalStatesBeforeTreatment.end(), m_vertexTranstionDetermAutomat[i]) == finalStatesBeforeTreatment.end()) {
				finalStatesBeforeTreatment.push_back(m_vertexTranstionDetermAutomat[i]);
			}
		}
	}
	for (size_t i = 0; i < finalStatesBeforeTreatment.size(); i++) {
		m_finalStatesOfDetermAutomat.push_back(getValueFromAlphabet(m_alphabet, finalStatesBeforeTreatment[i]));
	}
}

void CDeterminator::showNonDeterministicAutomat()
{
	for (size_t i = 0; i < m_tableTransitionsNonDetermAutomat.size(); i++) {
		for (size_t j = 0; j < m_tableTransitionsNonDetermAutomat[0].size(); j++) {
			for (size_t k = 0; k < m_tableTransitionsNonDetermAutomat[i][j].size(); k++) {
				std::cout << m_tableTransitionsNonDetermAutomat[i][j][k] << ' ';
			}
			std::cout << " | ";
		}
		std::cout << std::endl;
	}
}

void CDeterminator::showFinalStates()
{
	for (size_t i = 0; i < m_finalStatesOfNonDetermAutomat.size(); i++) {
		std::cout << m_finalStatesOfNonDetermAutomat[i] << ' ';
	}
}

void CDeterminator::showDeterministicAutomat()
{
	for (size_t i = 0; i < m_tableTransitionsDetermAutomat.size(); i++) {
		for (int l = 0; l < m_vertexTranstionDetermAutomat[i].size(); l++) {
			std::cout << m_vertexTranstionDetermAutomat[i][l];
		}
		std::cout << ": ";
		for (size_t j = 0; j < m_tableTransitionsDetermAutomat[0].size(); j++) {
			for (size_t k = 0; k < m_tableTransitionsDetermAutomat[i][j].size(); k++) {
				std::cout << m_tableTransitionsDetermAutomat[i][j][k] << ' ';
			}
			std::cout << " | ";
		}
		std::cout << std::endl;
	}
}

void CDeterminator::showRefactDeterministicAutomat(std::ofstream& output)
{
	output << m_xAmount << std::endl;
	output << m_determAutomat.size() << std::endl;
	output << m_finalStatesOfDetermAutomat.size() << std::endl;
	for (size_t i = 0; i < m_finalStatesOfDetermAutomat.size(); i++) {
		output << m_finalStatesOfDetermAutomat[i] << ' ';
	}
	output << std::endl;
	for (size_t i = 0; i < m_xAmount; i++) {
		for (size_t j = 0; j < m_determAutomat.size(); j++) {
			output << m_determAutomat[j][i] << ' ' ;
		}
		output << std::endl;
	}
}

void CDeterminator::createDotFile()
{
	std::ofstream dotFile("graph.dot");
	dotFile << "digraph DetermAutomat {" << std::endl;
	for (size_t i = 0; i < m_determAutomat.size(); i++) {
		if (std::find(m_finalStatesOfDetermAutomat.begin(), m_finalStatesOfDetermAutomat.end(), i) != m_finalStatesOfDetermAutomat.end()) {
			dotFile << i << " [shape = box]" << std::endl;
		}
		else {
			dotFile << i << std::endl;
		}
	}

	for (size_t i = 0; i < m_xAmount; i++) {
		for (size_t j = 0; j < m_determAutomat.size(); j++) {
			if (m_determAutomat[j][i] != -1) {
				dotFile << "	" << j << "->" << m_determAutomat[j][i] << "[label=" << i << ']' << std::endl;
			}
		}
	}
	dotFile << "}";
	dotFile.close();
}

CDeterminator::~CDeterminator()
{
}
