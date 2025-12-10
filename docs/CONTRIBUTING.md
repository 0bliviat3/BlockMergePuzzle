# Contributing to 2048 Collection

Thank you for your interest in contributing to 2048 Collection! 🎉

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)

## Code of Conduct

### Our Standards
- Be respectful and inclusive
- Welcome newcomers and help them learn
- Accept constructive criticism gracefully
- Focus on what's best for the project
- Show empathy towards others

### Unacceptable Behavior
- Harassment or discriminatory language
- Trolling or insulting comments
- Publishing others' private information
- Other unprofessional conduct

## Getting Started

### Prerequisites
- Unity 2022.3 or later
- Git for version control
- A GitHub account
- Basic knowledge of C# and Unity

### Fork and Clone
```bash
# Fork the repository on GitHub
# Then clone your fork
git clone https://github.com/YOUR_USERNAME/2048-collection.git
cd 2048-collection

# Add upstream remote
git remote add upstream https://github.com/ORIGINAL_OWNER/2048-collection.git
```

## Development Setup

### 1. Open in Unity
```
- Launch Unity Hub
- Click "Add" and select the project folder
- Open the project (may take a few minutes first time)
```

### 2. Install Dependencies
```
- LeanTween (if not included)
- Any required Unity packages
```

### 3. Test the Build
```
- Open MainMenu scene
- Press Play
- Test all game modes
- Check for console errors
```

## How to Contribute

### Reporting Bugs

Use the bug report template and include:
- Unity version
- Platform (Android/iOS/Editor)
- Steps to reproduce
- Expected vs actual behavior
- Screenshots or videos if applicable
- Console error messages

**Example:**
```markdown
**Bug Description:**
Tiles overlap when swiping quickly in Classic 2048

**Steps to Reproduce:**
1. Open Classic 2048 game
2. Swipe up rapidly 3 times
3. Observe tile positions

**Expected:** Tiles should move cleanly
**Actual:** Two tiles occupy the same cell

**Platform:** Android 12
**Unity Version:** 2022.3.10f1
```

### Suggesting Features

Use the feature request template:
- Clear description of the feature
- Why it would be useful
- How it might work
- Any alternatives considered

### Contributing Code

1. **Check existing issues** - See if your idea is already being worked on
2. **Open an issue** - Discuss major changes before coding
3. **Create a branch** - Use descriptive branch names
4. **Make your changes** - Follow coding standards
5. **Test thoroughly** - On device if possible
6. **Submit a PR** - With clear description

## Coding Standards

### C# Style Guide

#### Naming Conventions
```csharp
// Classes, Methods, Properties: PascalCase
public class GameManager { }
public void StartGame() { }
public int CurrentScore { get; set; }

// Private fields: camelCase with underscore
private int _playerScore;
private bool _isGameOver;

// Local variables, parameters: camelCase
int localVariable = 0;
void Method(int parameterName) { }

// Constants: UPPER_CASE
private const int MAX_SCORE = 9999;
```

#### Code Organization
```csharp
public class ExampleClass : MonoBehaviour
{
    // 1. Public fields
    public int publicField;
    
    // 2. Serialized private fields
    [SerializeField] private int serializedField;
    
    // 3. Private fields
    private int privateField;
    
    // 4. Properties
    public int Property { get; set; }
    
    // 5. Unity lifecycle methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    
    // 6. Public methods
    public void PublicMethod() { }
    
    // 7. Private methods
    private void PrivateMethod() { }
}
```

#### Comments and Documentation
```csharp
/// <summary>
/// Moves the tile to the specified position
/// </summary>
/// <param name="targetPosition">Target world position</param>
/// <param name="duration">Animation duration in seconds</param>
public void MoveTo(Vector2 targetPosition, float duration = 0.15f)
{
    // Cancel previous animations
    LeanTween.cancel(gameObject);
    
    // Start new animation
    LeanTween.move(rectTransform, targetPosition, duration)
        .setEase(LeanTweenType.easeOutQuad);
}
```

### Unity Best Practices

#### Performance
```csharp
// ✅ Good - Cache component references
private RectTransform _rectTransform;

private void Awake()
{
    _rectTransform = GetComponent<RectTransform>();
}

// ❌ Bad - Repeated GetComponent calls
private void Update()
{
    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
}
```

#### Memory Management
```csharp
// ✅ Good - Use object pooling for frequently created objects
private Queue<GameObject> _tilePool = new Queue<GameObject>();

// ✅ Good - Destroy objects when done
Destroy(oldTile.gameObject);

// ❌ Bad - Creating many temporary objects
private void Update()
{
    Vector2 temp = new Vector2(x, y); // Don't create in Update
}
```

#### Coroutines
```csharp
// ✅ Good - Store coroutine reference for stopping
private Coroutine _moveCoroutine;

private void StartMovement()
{
    if (_moveCoroutine != null)
        StopCoroutine(_moveCoroutine);
    
    _moveCoroutine = StartCoroutine(MoveRoutine());
}

// ✅ Good - Clean up coroutines
private void OnDestroy()
{
    if (_moveCoroutine != null)
        StopCoroutine(_moveCoroutine);
}
```

## Testing Guidelines

### Manual Testing Checklist

Before submitting a PR, test:

**Basic Functionality:**
- [ ] All games load without errors
- [ ] All buttons respond correctly
- [ ] Scene transitions work smoothly
- [ ] Audio plays correctly
- [ ] No console errors or warnings

**Classic 2048:**
- [ ] Swipe in all 4 directions
- [ ] Tiles merge correctly
- [ ] Score updates properly
- [ ] Game over detected correctly
- [ ] Best score saves and loads
- [ ] No diagonal movement
- [ ] No tile overlapping

**Block Merge Puzzle:**
- [ ] Blocks drop and place correctly
- [ ] Merge detection works
- [ ] Chain combos trigger
- [ ] Effects play smoothly
- [ ] Score tracks correctly
- [ ] Game over works

**Mobile Testing:**
- [ ] Test on actual device
- [ ] Check touch responsiveness
- [ ] Verify performance (60 FPS)
- [ ] Test different screen sizes
- [ ] Check memory usage

### Automated Testing

Currently, the project uses manual testing. Automated tests are planned for future releases.

## Commit Guidelines

### Commit Message Format
```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting)
- **refactor**: Code refactoring
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **chore**: Maintenance tasks

### Examples
```bash
# Good commit messages
feat(classic2048): add infinite play mode
fix(grid): resolve tile overlapping issue
docs(readme): update installation instructions
perf(animations): optimize LeanTween usage

# Bad commit messages
fixed stuff
update
changes
```

### Commit Best Practices
- Keep commits focused and atomic
- Write in present tense ("add" not "added")
- Reference issue numbers when applicable
- Explain *why* not just *what*

## Pull Request Process

### 1. Update Your Fork
```bash
git fetch upstream
git checkout main
git merge upstream/main
```

### 2. Create Feature Branch
```bash
git checkout -b feat/your-feature-name
```

### 3. Make Changes
- Write clean, documented code
- Follow coding standards
- Test thoroughly

### 4. Commit Changes
```bash
git add .
git commit -m "feat(scope): descriptive message"
```

### 5. Push to Your Fork
```bash
git push origin feat/your-feature-name
```

### 6. Open Pull Request
- Use the PR template
- Link related issues
- Add screenshots/videos if applicable
- Request review

### PR Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tested in Unity Editor
- [ ] Tested on Android
- [ ] Tested on iOS
- [ ] No console errors

## Screenshots
(if applicable)

## Related Issues
Fixes #123
```

### Review Process
1. Maintainer reviews code
2. Automated checks run (if configured)
3. Discussion and requested changes
4. Approval and merge

### After Merge
```bash
# Update your local main
git checkout main
git pull upstream main

# Delete feature branch
git branch -d feat/your-feature-name
git push origin --delete feat/your-feature-name
```

## Additional Resources

### Learning Resources
- [Unity Manual](https://docs.unity3d.com/Manual/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Git Documentation](https://git-scm.com/doc)

### Project Resources
- [README.md](README.md) - Project overview
- [STRUCTURE.md](STRUCTURE.md) - Project structure
- [CHANGELOG.md](CHANGELOG.md) - Version history

### Community
- GitHub Issues - Bug reports and discussions
- GitHub Discussions - General questions
- Pull Requests - Code contributions

## Questions?

If you have questions:
1. Check existing issues and discussions
2. Read the documentation
3. Open a new issue with the "question" label

## Recognition

Contributors will be recognized in:
- README.md credits section
- Release notes
- CHANGELOG.md

Thank you for contributing! 🎉

---

*Last Updated: December 2025*
