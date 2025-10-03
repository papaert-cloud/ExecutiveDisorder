# Executive Disorder - Game Design Document

## 🎯 Core Concept

A satirical political decision-making game where players take on the role of a world leader navigating impossible choices. Every decision creates dramatic consequences, cascade effects, and media reactions that affect four critical resource meters.

## 🎮 Gameplay Loop

### 1. Decision Phase
- Player is presented with a crisis/situation card
- Two options are always available
- Each option has hidden consequences
- Visual representation via card UI

### 2. Consequence Phase
- Resources are modified based on choice
- Visual effects trigger (explosions, celebrations, alerts)
- Audio feedback plays
- News headlines appear
- Social media reactions display

### 3. Cascade Phase (30% chance)
- Some decisions trigger follow-up cards
- Create chain reactions of events
- Escalate or de-escalate situations
- Add unpredictability to outcomes

### 4. Game Over Check
- Any resource reaching 0 = immediate game over
- 50 decisions = successful term completion
- Ending determined by final resource state

## 📊 Resource System

### Four Core Resources (0-100)
Each starts at 50 and must be carefully balanced:

1. **Popularity** 👥
   - Public approval rating
   - Affects re-election chances
   - Influenced by public-facing decisions
   - Lost through unpopular choices
   - Critical for legitimacy

2. **Stability** 🏛️
   - National order and security
   - Social cohesion indicator
   - Lost through chaotic decisions
   - Gained through law and order
   - Critical for governance

3. **Media Support** 📺
   - Press relations
   - Communication capability
   - Information control
   - Lost through censorship or scandals
   - Critical for messaging

4. **Economic Health** 💰
   - Financial stability
   - Market confidence
   - Job security
   - Lost through poor economic choices
   - Critical for funding programs

### Resource Interdependencies
- High popularity can offset low stability
- Strong economy can compensate for low media
- All resources affect ending quality
- Balance is key to success

## 🎭 Character System

### 8 Political Leaders
Each with unique traits and influence modifiers:

1. **Victoria Steele** - Progressive Reformer
   - +10% Popularity, -10% Stability
   - Favors social change over order

2. **Marcus Wellington** - Conservative Traditionalist
   - +10% Stability, -15% Economic
   - Favors tradition over progress

3. **Dr. Sarah Chen** - Scientific Pragmatist
   - +20% Media, -10% Popularity
   - Data-driven decision making

4. **General James Hawk** - Military Strategist
   - +15% Stability, -10% Media
   - Security-first approach

5. **Isabella Rodriguez** - Social Justice Advocate
   - +10% Popularity, -10% Economic
   - Equity over efficiency

6. **Professor David Klein** - Economic Theorist
   - +20% Economic, -15% Popularity
   - Markets over sentiment

7. **Ambassador Fatima Hassan** - Diplomatic Negotiator
   - +10% Media, +10% Stability
   - Balanced approach

8. **Senator Thomas Burke** - Populist Firebrand
   - +20% Popularity, -20% Media
   - People over institutions

## 📝 Decision Card Structure

### Card Components
- **ID**: Unique identifier (1-101)
- **Title**: Catchy crisis name
- **Description**: Situation context (3-5 sentences)
- **Image**: Visual representation (optional)
- **Option 1**: First choice with consequences
- **Option 2**: Alternative choice with consequences

### Consequence Types
1. **Resource Changes** (-25 to +25)
2. **Visual Effects** (explosion, celebration, crisis)
3. **Audio Cues** (alarm, applause, explosion)
4. **News Headlines** (breaking news style)
5. **Social Media** (trending hashtags, reactions)

### Card Categories (101 Total)

#### Political (20 cards)
- Nuclear crisis, war decisions
- International relations
- Constitutional issues
- Election integrity
- Government reform

#### Economic (20 cards)
- Stimulus packages, tax reform
- Trade agreements
- Labor issues
- Market regulation
- Debt management

#### Social (20 cards)
- Healthcare, education
- Civil rights, immigration
- Gun control, abortion
- Police reform
- Religious freedom

#### Environmental (15 cards)
- Climate change
- Renewable energy
- Conservation
- Pollution control
- Natural disasters

#### Technology (15 cards)
- AI regulation
- Privacy laws
- Cybersecurity
- Space exploration
- Social media control

#### Satirical/Absurd (11 cards)
- Time travel regulation
- Alien contact protocol
- Clone rights
- Telepathy laws
- Weather control ethics

## 🏆 Ending System

### 10 Distinct Endings

#### Success Tier
1. **Golden Age** (Avg 80+)
   - "History remembers you as legendary"
   - All resources high
   - Maximum prestige

2. **Successful Term** (Avg 70-79)
   - "Wise leadership prevails"
   - Strong performance
   - Positive legacy

3. **Balanced Leadership** (Avg 60-69)
   - "Maintained delicate balance"
   - Acceptable performance
   - Mixed legacy

#### Struggle Tier
4. **Struggling Through** (Avg 50-59)
   - "Nation survives, barely"
   - Weak performance
   - Questionable legacy

5. **Constant Crisis** (Avg 40-49)
   - "Crisis after crisis"
   - Poor performance
   - Negative legacy

6. **Complete Disaster** (Avg <40)
   - "Name synonymous with failure"
   - Terrible performance
   - Infamous legacy

#### Specific Failure
7. **Overthrown** (Popularity = 0)
   - "The people rose up"
   - Popular revolution
   - Forced removal

8. **Nation Collapsed** (Stability = 0)
   - "Society broke down"
   - Total chaos
   - State failure

9. **Media Blackout** (Media = 0)
   - "Lost all communication"
   - Information war lost
   - Isolation

10. **Economic Ruin** (Economic = 0)
    - "Financial collapse"
    - Market devastation
    - Bankruptcy

## 🎬 Opening Sequence

### Nuclear Crisis Opening
The game begins with maximum drama:

1. **Alert Screen**
   - Red flashing visuals
   - Urgent alarm audio
   - Crisis text appears

2. **Situation Brief**
   - "Mr. President, emergency!"
   - Intelligence reports
   - Military advisors calling for action
   - Media already speculating
   - Markets plummeting

3. **Tension Building**
   - Timer counting down (visual)
   - Multiple perspectives shown
   - No correct answer presented
   - Player must decide

4. **Transition to Gameplay**
   - Auto-progress after 8 seconds
   - Or manual continue
   - Sets tone for entire game

## 💥 Consequence Cascade System

### How Cascades Work
1. **Trigger Probability**: 30% default per decision
2. **Cascade Pool**: Each card has 0-3 linked cards
3. **Selection**: Random from cascade pool
4. **Insertion**: Added to top of deck (immediate)
5. **Chain Reactions**: Cascades can trigger cascades

### Cascade Examples
- Nuclear strike → Refugee crisis
- Economic collapse → Social unrest
- Climate disaster → Migration wave
- War declaration → Alliance breakdown
- Healthcare crisis → Economic strain

### Cascade Design Philosophy
- Mirrors real political consequences
- No decision exists in isolation
- Unintended consequences are the norm
- Creates emergent storytelling
- Increases replay value

## 📰 News & Social Media System

### Breaking News Headlines
- Appear after each decision
- Ticker-style animation
- Stay visible for 3 seconds
- Stored in history (last 10)
- Reflect decision impact

### Social Media Reactions
- Hashtag trending (#WorldWar3)
- User reactions with emojis
- Mix of support and outrage
- Stored in feed (last 15)
- Scroll through history

### Media Types
**Positive:**
- "Historic decision!"
- "Leadership at its finest!"
- "Finally, someone who gets it!"
- "#BestPresidentEver trending"

**Negative:**
- "Controversial decision sparks outrage"
- "Critics condemn action"
- "What were they thinking?!"
- "#WorstDecisionEver trending"

**Neutral:**
- "President announces policy"
- "Mixed reactions to decision"
- "Experts debate implications"

## 🎨 Visual & Audio Feedback

### Visual Effects
1. **Explosion** - Red flash, particles
   - War, conflict, disaster decisions
2. **Celebration** - Gold particles, confetti
   - Successful, popular decisions
3. **Crisis** - Screen shake, dark overlay
   - Negative, dangerous decisions

### Audio Cues
1. **Alarm** - Urgent siren
   - Warnings, emergencies
2. **Applause** - Crowd cheering
   - Victories, popular choices
3. **Explosion** - Boom sound
   - Catastrophic events
4. **Positive** - Pleasant chime
   - Good outcomes

### UI Animation
- Card slide in/out
- Resource bar smooth transitions
- Headline ticker scroll
- Flash overlay fade
- Button hover effects

## 🌐 WebGL Optimization

### Size Constraints (<50MB)
1. **No Textures** - Use procedural/UI
2. **Minimal Audio** - Essential sounds only
3. **Code Only** - ScriptableObject data
4. **Brotli Compression** - Max compression
5. **Stripped Builds** - Remove unused code

### Performance Targets
- 60 FPS on modern browsers
- <256MB memory usage
- <2 second load time
- Mobile responsive

### Browser Compatibility
- Chrome 90+ ✅
- Firefox 88+ ✅
- Safari 14+ ✅
- Edge 90+ ✅

## 🔄 Replayability Features

### Variation Factors
1. **Different Characters** - 8 unique starts
2. **Card Shuffle** - Random order each game
3. **Cascade Randomness** - 30% trigger chance
4. **Multiple Endings** - 10 possible outcomes
5. **Strategy Diversity** - Many viable approaches

### Encouragement
- "Try different character"
- "Discover all endings"
- "Survive longer"
- "Find optimal strategy"
- "Experience all cards"

## 📈 Balancing Philosophy

### Difficulty Curve
- Early cards: Moderate consequences
- Mid game: Increasing complexity
- Late game: Cascade chains intensify
- No perfect strategy exists
- Failure is part of the experience

### Resource Philosophy
- No resource is "most important"
- Neglecting any leads to game over
- Balancing is the core challenge
- Trade-offs are necessary
- Recovery is always possible until 0

## 🎭 Satirical Elements

### Comedy Through
1. **Absurd Situations** - Time travel regulation
2. **Impossible Choices** - Damned if you do...
3. **Exaggerated Reactions** - Media hysteria
4. **Political Archetypes** - Stereotypical advisors
5. **Real-World Parallels** - Obvious references

### Tone Balance
- Serious mechanics
- Humorous presentation
- Dark comedy elements
- Social commentary
- Player agency respected

## 🚀 Future Expansions

### Potential Features
- [ ] Multiplayer voting on decisions
- [ ] Historical playthrough mode
- [ ] Achievement system
- [ ] Decision tree visualization
- [ ] Modding support
- [ ] Character portraits/animations
- [ ] Voice acting
- [ ] Additional card packs (DLC)
- [ ] Scenario editor
- [ ] Leaderboards

### Community Content
- Player-created cards
- Custom characters
- Alternate endings
- Balance mods
- Total conversions

---

**Executive Disorder** - Where every decision is wrong, but you have to choose anyway! 🎮🏛️

*"In politics, there are no right answers... only less wrong ones."*
