using UnityEngine;
using System.Linq;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;   // Scroll View의 Content
    [SerializeField] GameObject rowPrefab;    // RankRow 프리팹

    StageResultList allData;
    int currentStage = 1;                     // 현재 선택된 Stage

    void Awake()
    {
        allData = StageResultSaver.LoadRank();
    }

    void OnEnable()
    {
        // 랭킹 페이지를 열 때마다 최신 데이터 다시 불러오기
        allData = StageResultSaver.LoadRank();
        RefreshRankList();
    }

    // 버튼에서 호출할 함수
    public void SetStage(int stage)
    {
        currentStage = stage;
        RefreshRankList();
    }

    void RefreshRankList()
    {
        // 기존 랭킹 삭제
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        // 선택된 Stage의 데이터만 추출 후 점수 내림차순 정렬
        var sortedData = allData.results
            .Where(r => r.stage == currentStage)
            .OrderByDescending(x => x.score)
            .ToList();

        // 데이터가 없으면 안내 문구 표시
        if (sortedData.Count == 0)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = "기록이 없습니다.";
            return;
        }

        // 랭킹 표시
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text =
                $"{i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}