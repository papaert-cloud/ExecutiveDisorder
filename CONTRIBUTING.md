# Contributing to Executive Disorder

Thank you for your interest in contributing to Executive Disorder! This document provides guidelines for contributing to the project.

## 🎯 Ways to Contribute

### 1. Decision Cards
Create new political scenarios and dilemmas:
- Must have clear title and description
- Two distinct options with consequences
- Impact on at least 2 resources
- Include news headline and social media reaction
- Optional: Add cascade cards for chain effects

### 2. Political Characters
Design new playable leaders:
- Unique name and title
- Clear ideology and biography
- Balanced influence modifiers (0.8-1.2 range)
- Distinct personality traits

### 3. Endings
Propose new ending scenarios:
- Clear trigger conditions
- Compelling title and description
- Appropriate for the resource state
- Consistent with game's satirical tone

### 4. Code Improvements
Enhance game systems:
- Bug fixes
- Performance optimizations
- New features
- UI/UX improvements

### 5. Documentation
Improve project documentation:
- Fix typos and errors
- Add examples and tutorials
- Translate to other languages
- Create video guides

## 📋 Contribution Process

### For Content (Cards, Characters, Endings)

1. **Fork the Repository**
   ```bash
   git fork https://github.com/papaert-cloud/ExecutiveDisorder
   ```

2. **Create Content Branch**
   ```bash
   git checkout -b content/your-content-name
   ```

3. **Add Your Content**
   - Use Unity Editor: Right-click > Create > Executive Disorder > [Card/Character]
   - Or use the Pipeline tool: Tools > AI Agent > Exec Full Pipeline
   - Place in appropriate Resources folder

4. **Test Your Content**
   - Load in Unity Editor
   - Play through your card/character
   - Verify consequences work correctly
   - Check for typos and balance

5. **Submit Pull Request**
   - Include description of content
   - Mention any balance considerations
   - Add screenshots if applicable

### For Code Changes

1. **Create Issue First**
   - Describe the problem or feature
   - Get feedback from maintainers
   - Discuss implementation approach

2. **Fork and Branch**
   ```bash
   git checkout -b feature/your-feature-name
   # or
   git checkout -b fix/your-bug-fix
   ```

3. **Follow Code Standards**
   - Use C# naming conventions
   - Add XML documentation comments
   - Keep methods small and focused
   - Write self-documenting code

4. **Test Thoroughly**
   - Test in Unity Editor
   - Test WebGL build
   - Check for console errors
   - Verify no performance regression

5. **Submit Pull Request**
   - Clear title and description
   - Link related issues
   - Include testing steps

## 🎨 Content Guidelines

### Decision Cards

**Good Example:**
```
Title: "Alien Contact Protocol"
Description: "First contact! Alien spacecraft detected. Military wants to shoot first. Scientists want to communicate."
Option 1: "Attempt Communication" → +Media, -Stability
Option 2: "Military Response" → +Stability, -Media
```

**Bad Example:**
```
Title: "Thing"
Description: "Stuff happens"
Option 1: "Yes" → Everything changes randomly
Option 2: "No" → Nothing happens
```

### Character Design

**Good Example:**
```
Name: "Dr. Maya Singh"
Title: "The Tech Futurist"
Ideology: "Technocratic"
Influences: +20% Media, -10% Stability
Bio: "Former Silicon Valley CEO turned politician..."
```

**Bad Example:**
```
Name: "Joe"
Title: "Guy"
Ideology: "Stuff"
Influences: All +50%
```

## 💻 Code Standards

### C# Style
```csharp
// Good: Clear, documented, follows conventions
/// <summary>
/// Modifies a specific resource by the given amount.
/// </summary>
/// <param name="resourceName">Name of the resource to modify</param>
/// <param name="amount">Amount to add/subtract</param>
public void ModifyResource(string resourceName, float amount)
{
    switch (resourceName.ToLower())
    {
        case "popularity":
            popularity = Mathf.Clamp(popularity + amount, MIN_VALUE, MAX_VALUE);
            break;
    }
}

// Bad: Unclear, undocumented, poor naming
public void DoThing(string s, float f)
{
    if (s == "p") p = p + f;
}
```

### Unity Best Practices
- Use `[SerializeField]` for private fields in Inspector
- Avoid `FindObjectOfType` in Update/FixedUpdate
- Use object pooling for frequently created objects
- Cache component references
- Use coroutines for sequences

## 🧪 Testing Requirements

### For Content
- [ ] Card displays correctly
- [ ] Both options are viable
- [ ] Consequences are balanced
- [ ] No spelling/grammar errors
- [ ] Cascades work if applicable

### For Code
- [ ] No compiler errors
- [ ] No runtime errors
- [ ] No console warnings
- [ ] Performance is acceptable
- [ ] Works in WebGL build

## 📝 Pull Request Template

When submitting, include:

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Decision Card
- [ ] Character
- [ ] Ending
- [ ] Bug Fix
- [ ] Feature
- [ ] Documentation

## Testing
How you tested the changes

## Screenshots
If applicable

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings
- [ ] Tested in Unity Editor
- [ ] Tested WebGL build
```

## 🚫 What NOT to Contribute

### Avoid These
- ❌ Offensive or discriminatory content
- ❌ Real political figures (use parodies/archetypes)
- ❌ Copyrighted material
- ❌ Unbalanced content (all +100 or all -100)
- ❌ Breaking changes without discussion
- ❌ Large refactors without approval
- ❌ Binaries or large assets to repo

### Instead
- ✅ Satirical fictional scenarios
- ✅ Generic political archetypes
- ✅ Original content
- ✅ Balanced gameplay (trade-offs)
- ✅ Incremental improvements
- ✅ Small, focused changes
- ✅ Use .gitignore for assets

## 🎯 Balance Guidelines

### Resource Changes
- Small impact: ±5 to ±10
- Medium impact: ±15 to ±20
- Large impact: ±25 to ±30
- Extreme: ±35 to ±40 (rare!)

### Cascade Probability
- Low: 0.2 (20%)
- Normal: 0.3 (30%)
- High: 0.4 (40%)
- Very High: 0.5+ (50%+)

### Character Influences
- Minimal: 0.9 to 1.1
- Moderate: 0.8 to 1.2
- Strong: 0.7 to 1.3
- Extreme: 0.5 to 1.5 (very rare!)

## 📊 Review Process

1. **Automated Checks**
   - Build succeeds
   - No merge conflicts
   - File size limits

2. **Maintainer Review**
   - Code quality
   - Content appropriateness
   - Balance considerations
   - Documentation completeness

3. **Community Feedback**
   - Other contributors may comment
   - Suggestions for improvement
   - Testing by others

4. **Approval and Merge**
   - At least one maintainer approval
   - All feedback addressed
   - CI/CD passes
   - Merged to main branch

## 🏆 Recognition

Contributors will be:
- Added to CONTRIBUTORS.md
- Credited in game if significant contribution
- Mentioned in release notes
- Given credit in documentation

## 📞 Getting Help

- **Questions**: Open a GitHub Discussion
- **Bugs**: Open an Issue
- **Ideas**: Start a Discussion
- **Urgent**: Contact maintainers directly

## 🎮 Special Projects

Looking for bigger challenges? Consider:

### Card Pack Themes
- Historical Events Pack
- Sci-Fi Scenarios Pack
- Environmental Crisis Pack
- Tech Revolution Pack

### New Systems
- Achievement system
- Difficulty modes
- Historical replay mode
- Multiplayer voting

### Localizations
- Translate UI to other languages
- Adapt content for different cultures
- Create region-specific cards

## 📜 Code of Conduct

### Be Respectful
- Treat others with kindness
- Accept constructive criticism
- Give constructive feedback
- Respect different viewpoints

### Be Professional
- Keep discussions on-topic
- No spam or self-promotion
- No harassment or trolling
- No personal attacks

### Be Collaborative
- Help newcomers
- Share knowledge
- Credit others' work
- Work towards common goals

## ⚖️ License

By contributing, you agree that your contributions will be licensed under the same license as the project.

---

**Thank you for helping make Executive Disorder better!** 🎮🏛️

Every decision you contribute makes the game more engaging, challenging, and satirically delightful!
