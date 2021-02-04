#include <string>
#include <vector>
#include <sstream>
#include <algorithm>

using namespace std;

int strToIdx(string str)
{
    if (str.compare("cpp") == 0 || str.compare("backend") == 0 ||
        str.compare("junior") == 0 || str.compare("chicken") == 0)
        return 0;
    else if (str.compare("java") == 0 || str.compare("frontend") == 0 ||
        str.compare("senior") == 0 || str.compare("pizza") == 0)
        return 1;
    else if (str.compare("python") == 0) return 2;
    else return 3;
}

vector<int> solution(vector<string> info, vector<string> query) {
    vector<int> answer;
    vector<int> scores[24];
    stringstream stream;

    for (int i = 0; i < info.size(); i++)
    {
        stream.str(info[i]);
        int score, idx[4];
        for (int j = 0; j < 4; j++)
        {
            string temp;
            stream >> temp;
            idx[j] = strToIdx(temp);
        }
        stream >> score;
        scores[idx[0] * 8 + idx[1] * 4 + idx[2] * 2 + idx[3]].push_back(score);
        stream.clear();
    }

    for (int i = 0; i < 24; i++)
        sort(scores[i].begin(), scores[i].end());

    for (int i = 0; i < query.size(); i++)
    {
        stream.str(query[i]);
        int score, idx;
        vector<int> queryIndices;
        int count = 8; //배수
        string temp;
        for (int j = 0; j < 4; j++)
        {
            stream >> temp;
            idx = strToIdx(temp);

            if (idx < 3) //특정될때
            {
                if (j == 0)
                    queryIndices.push_back(count * idx);
                else
                    for (int k = 0; k < queryIndices.size(); k++)
                        queryIndices[k] += count * idx;
            }
            else    //해당 항목의 모두 검색할때
            {
                if (j == 0)
                    for (int k = 0; k < 3; k++)
                        queryIndices.push_back(count * k);
                else
                {
                    int max = queryIndices.size();
                    for (int k = 0; k < max; k++)
                        queryIndices.push_back(queryIndices[k] + count);
                }
            }

            count /= 2;
            if (j < 3) stream >> temp;   //and제거
        }

        stream >> score; //점수 갖고오기

        //lower-bound 이용해 이분탐색
        int queryCount = 0;
        for (int j = 0; j < queryIndices.size(); j++)
        {
            if (scores[queryIndices[j]].size() == 0) continue;

            int mid, low, high;
            low = 0, high = scores[queryIndices[j]].size();

            while (low < high)
            {
                mid = (low + high) / 2;
                if (scores[queryIndices[j]][mid] >= score) high = mid;
                else low = mid + 1;
            }

            queryCount += (scores[queryIndices[j]].size() - high);
        }
        answer.push_back(queryCount);

        stream.clear();
    }

    return answer;
}