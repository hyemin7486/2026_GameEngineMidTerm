using UnityEngine;
using System.Linq;
using TMPro;

public class RankPage : MonoBehaviour
{

    [SerializeField] Transform contentRoot;           // Content 오브젝트
    [SerializeField] GameObject rowPrefab;            // RankRow 프리팹

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();
        RefreshRankList();
    }

    void RefreshRankList()
    {
        // 기존의 모든 자식 오브젝트 삭제
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        // 랭크 데이터 정렬
        var sortedData = allData.results.Where(r => r.stage == 1).OrderByDescending(x => x.score).ToList();

        // 랭크 데이터 생성
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}
