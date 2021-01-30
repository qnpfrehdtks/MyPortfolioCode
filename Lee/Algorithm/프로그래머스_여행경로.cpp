#include <string>
#include <vector>
#include <algorithm>

using namespace std;

bool dfs(vector<string>& answer, vector<vector<string>>& tickets, vector<bool>& used, string airport)
{
    answer.push_back(airport);
    if (answer.size() == tickets.size() + 1) return true;

    for (int i = 0; i < tickets.size(); i++)
    {
        if ((tickets[i][0].compare(airport) != 0) || used[i]) continue;

        used[i] = true;
        if (dfs(answer, tickets, used, tickets[i][1])) return true;
        used[i] = false;
    }

    answer.pop_back();
    return false;
}


vector<string> solution(vector<vector<string>> tickets) {
    vector<string> answer;
    vector<bool> used(tickets.size(), false);

    sort(tickets.begin(), tickets.end());
    dfs(answer, tickets, used, "ICN");
    return answer;
}