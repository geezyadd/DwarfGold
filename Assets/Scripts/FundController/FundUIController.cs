using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class FundUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentRedText;
    [SerializeField] private TextMeshProUGUI _currentBlueText;
    [SerializeField] private TextMeshProUGUI _totalRedText;
    [SerializeField] private TextMeshProUGUI _totalBlueText;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private TextMeshProUGUI _winnerText;
    private FundController _fundController;

    [Inject]
    private void InjectDependencies(FundController fundController)
    {
        _fundController = fundController;
    }

    private void FixedUpdate()
    {
        _currentRedText.SetText(_fundController.CurrentRedFund.ToString());
        _currentBlueText.SetText(_fundController.CurrentBlueFund.ToString());
        _totalRedText.SetText(_fundController.TotalRedFund.ToString());
        _totalBlueText.SetText(_fundController.TotalBlueFund.ToString());
        if (_fundController.IsEndGame)
        {
            _endGameUI.SetActive(true);
            _winnerText.SetText(_fundController.Winner.ToString() + " is winner!" );
        }
    }
}
