using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using ExecutiveDisorder.Avalonia.Models;

namespace ExecutiveDisorder.Avalonia.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private Character? _selectedCharacter;
    private DecisionCard? _currentCard;
    private List<DecisionCard> _allCards = new();
    private List<Ending> _allEndings = new();
    private int _popularity;
    private int _stability;
    private int _mediaTrust;
    private int _economic;
    private bool _isGameStarted;
    private bool _isGameOver;
    private string _endingTitle = string.Empty;
    private string _endingDescription = string.Empty;
    private string _gameStatus = "Select a character to begin your political journey...";
    private Random _random = new();
    private HashSet<int> _usedCardIds = new();

    public ObservableCollection<Character> Characters { get; } = new();

    public Character? SelectedCharacter
    {
        get => _selectedCharacter;
        set { _selectedCharacter = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanStartGame)); }
    }

    public DecisionCard? CurrentCard
    {
        get => _currentCard;
        set { _currentCard = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasCurrentCard)); }
    }

    public int Popularity
    {
        get => _popularity;
        set { _popularity = Math.Clamp(value, 0, 100); OnPropertyChanged(); }
    }

    public int Stability
    {
        get => _stability;
        set { _stability = Math.Clamp(value, 0, 100); OnPropertyChanged(); }
    }

    public int MediaTrust
    {
        get => _mediaTrust;
        set { _mediaTrust = Math.Clamp(value, 0, 100); OnPropertyChanged(); }
    }

    public int Economic
    {
        get => _economic;
        set { _economic = Math.Clamp(value, 0, 100); OnPropertyChanged(); }
    }

    public bool IsGameStarted
    {
        get => _isGameStarted;
        set { _isGameStarted = value; OnPropertyChanged(); }
    }

    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); }
    }

    public string EndingTitle
    {
        get => _endingTitle;
        set { _endingTitle = value; OnPropertyChanged(); }
    }

    public string EndingDescription
    {
        get => _endingDescription;
        set { _endingDescription = value; OnPropertyChanged(); }
    }

    public string GameStatus
    {
        get => _gameStatus;
        set { _gameStatus = value; OnPropertyChanged(); }
    }

    public bool CanStartGame => SelectedCharacter != null && !IsGameStarted;
    public bool HasCurrentCard => CurrentCard != null && !IsGameOver;

    public ICommand StartGameCommand { get; }
    public ICommand MakeDecisionCommand { get; }
    public ICommand NewGameCommand { get; }

    public MainWindowViewModel()
    {
        StartGameCommand = new RelayCommand(StartGame, () => CanStartGame);
        MakeDecisionCommand = new RelayCommand<ChoiceOption>(MakeDecision);
        NewGameCommand = new RelayCommand(NewGame);

        LoadGameData();
    }

    private void LoadGameData()
    {
        try
        {
            // Load characters
            var charactersJson = File.ReadAllText(Path.Combine("Data", "charactersjson.json"));
            var charactersData = JsonSerializer.Deserialize<CharactersData>(charactersJson);
            if (charactersData?.Characters != null)
            {
                foreach (var character in charactersData.Characters)
                {
                    Characters.Add(character);
                }
            }

            // Load cards
            var cardsJson = File.ReadAllText(Path.Combine("Data", "cardsjson.json"));
            var cardsData = JsonSerializer.Deserialize<CardsData>(cardsJson);
            _allCards = cardsData?.Cards ?? new List<DecisionCard>();

            // Load endings
            var endingsJson = File.ReadAllText(Path.Combine("Data", "endingjson.json"));
            var endingsData = JsonSerializer.Deserialize<EndingsData>(endingsJson);
            _allEndings = endingsData?.Endings ?? new List<Ending>();

            GameStatus = $"Loaded {Characters.Count} characters, {_allCards.Count} cards, {_allEndings.Count} endings. Ready to start!";
        }
        catch (Exception ex)
        {
            GameStatus = $"Error loading game data: {ex.Message}";
        }
    }

    private void StartGame()
    {
        if (SelectedCharacter == null) return;

        IsGameStarted = true;
        IsGameOver = false;
        _usedCardIds.Clear();

        Popularity = SelectedCharacter.InitialPopularity;
        Stability = SelectedCharacter.InitialStability;
        MediaTrust = SelectedCharacter.InitialMedia;
        Economic = SelectedCharacter.InitialEconomic;

        GameStatus = $"Playing as {SelectedCharacter.CharacterName} - {SelectedCharacter.CampaignSlogan}";
        
        LoadNextCard();
    }

    private void LoadNextCard()
    {
        if (_allCards.Count == 0) return;

        var availableCards = _allCards.Where(c => !_usedCardIds.Contains(c.CardID)).ToList();
        
        if (availableCards.Count == 0)
        {
            // All cards used, game ends
            CheckGameEnding();
            return;
        }

        var card = availableCards[_random.Next(availableCards.Count)];
        _usedCardIds.Add(card.CardID);
        CurrentCard = card;
    }

    private void MakeDecision(ChoiceOption? option)
    {
        if (option == null || CurrentCard == null) return;

        // Apply resource changes
        foreach (var req in option.ResourceRequirements)
        {
            switch (req.Type.ToLower())
            {
                case "popularity":
                    Popularity += (int)req.Amount;
                    break;
                case "stability":
                    Stability += (int)req.Amount;
                    break;
                case "media":
                    MediaTrust += (int)req.Amount;
                    break;
                case "economic":
                    Economic += (int)req.Amount;
                    break;
            }
        }

        // Check for game over conditions
        if (Popularity <= 0 || Stability <= 0 || MediaTrust <= 0 || Economic <= 0)
        {
            EndingTitle = "IMPEACHED!";
            EndingDescription = "Your administration has collapsed. One of your key resources has been completely depleted.";
            IsGameOver = true;
            return;
        }

        // Check for winning conditions
        CheckGameEnding();

        if (!IsGameOver)
        {
            LoadNextCard();
        }
    }

    private void CheckGameEnding()
    {
        foreach (var ending in _allEndings)
        {
            bool meetsRequirements = true;

            foreach (var req in ending.ResourceRequirements)
            {
                int currentValue = req.ResourceType.ToLower() switch
                {
                    "popularity" => Popularity,
                    "stability" => Stability,
                    "media" => MediaTrust,
                    "economic" => Economic,
                    _ => 0
                };

                bool conditionMet = req.Comparison.ToLower() switch
                {
                    "greaterthan" => currentValue > req.Value,
                    "lessthan" => currentValue < req.Value,
                    "equals" => currentValue == req.Value,
                    _ => false
                };

                if (!conditionMet)
                {
                    meetsRequirements = false;
                    break;
                }
            }

            if (meetsRequirements)
            {
                EndingTitle = ending.Title;
                EndingDescription = ending.Description;
                IsGameOver = true;
                return;
            }
        }
    }

    private void NewGame()
    {
        IsGameStarted = false;
        IsGameOver = false;
        CurrentCard = null;
        SelectedCharacter = null;
        _usedCardIds.Clear();
        GameStatus = "Select a character to begin your political journey...";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => _execute();

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}

public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Func<T?, bool>? _canExecute;

    public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;
    public void Execute(object? parameter) => _execute((T?)parameter);

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }
}
