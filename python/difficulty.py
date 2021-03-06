# Tuning the difficulty in 'Personal Space' will be critical to the game's appeal/success
# The game must begin easy enough for everyone, so no one is put off by it being too hard.
# Then, the game must quickly become more difficult as time progresses.
# If the game becomes difficult too fast, people will loose interest as improvement yields little score increase.
# If the game becomes difficult too slowly, people will loose interest due to the length of games.

# Target difficulties are outlined for different time periods below.
# Game difficulty parameters are tuned to accomplish the noted difficulties.

# 0 minutes
# The game should start out playable for all beginners, so no one is put off by it being too hard

# 0.5 minutes
# Beginners should rarely if ever make it past the 30 second mark

# 1 minute
# People should start surviving 1 minute after a total of 10 minutes experience.

# 2 minutes
# Reached after 1 hour of experience.

# 3 minutes
# Reached after many (10?) hours of experience.

# 5 minutes
# Only incredibly skilled players should reach 5 minutes. And even then only on their best games.

# infinity
# No matter how good someone gets, they should never be able to play forever.
# The game should 'asymptote' to a humanly impossible level of difficulty.

import numpy as np
import matplotlib
import matplotlib.pyplot as plt

font = {'family' : 'normal',
        'weight' : 'bold',
        'size'   : 16}

matplotlib.rc('font', **font)

minutes = np.arange(0, 5, 0.01)

# ---------------     V1     --------------

_startPlayerSpeed = 6 #units per second
_startEnemySpeed = 3 #units per second
_startEnemySpawn = 2 #seconds per spawn

_modPlayerSpeed = 0.2 #change per minute
_modEnemySpeed = 0.2 #change per minute
_modEnemySpawn = -0.2 #change per minute

# y = mx + b
def linear(x, m, b):
  return m * x + b

# _playerSpeed = linear(minutes, _modPlayerSpeed, _startPlayerSpeed)
# _enemySpeed = linear(minutes, _modEnemySpeed, _startEnemySpeed)
# _enemySpawn = linear(minutes, _modEnemySpawn, _startEnemySpawn)

# ---------------     V2     --------------

def asymptotic(x, asymptote, xShift, yScale, power):
  return yScale * 1/(x + xShift)**power + asymptote

# # (x, asymptote, xShift, yScale, power)
# playerSpeed = asymptotic(minutes, 10, 1.5, -4, .75)
# enemySpeed = asymptotic(minutes, 6, 1.5, -3, .75)
# enemySpawn = asymptotic(minutes, 0.5, 2, 1, 1)






# ---------------     V3     -----------------

def asymptotic2(x, initY, asymptote, pow):
  scale = initY - asymptote
  return scale / (x + 1) ** pow + asymptote

# (x, initY, asymptote, pow)
playerSpeed = asymptotic2(minutes, 5, 10.5, .55)
enemySpeed = asymptotic2(minutes, 2, 6, .55)
enemySpawn = asymptotic2(minutes, 3, 0.5, 1.2)








# Old Values
_playerSpeed = asymptotic2(minutes, 5, 10.5, .65)
_enemySpeed = asymptotic2(minutes, 2, 6, .65)
_enemySpawn = asymptotic2(minutes, 3, 0.5, 1.0)

fig, (ax1, ax2) = plt.subplots(2, sharex=True)
fig.suptitle('Difficulty Parameters over Time')

ax1.plot(minutes, playerSpeed, 'b-')
ax1.plot(minutes, enemySpeed, 'r-')
ax1.plot(minutes, _playerSpeed, 'b:')
ax1.plot(minutes, _enemySpeed, 'r:')
ax1.set(ylabel='Character Speed', xlabel='Minutes of Play')
ax1.legend(["Player", "Enemy"],loc="upper left")

ax2.plot(minutes, _enemySpawn, 'r:')
ax2.plot(minutes, enemySpawn, 'r-')
ax2.set(ylabel='Enemy Spawn Time', xlabel='Minutes of Play')
ax1.grid(b=True, which='both', axis='both')
ax2.grid(b=True, which='both', axis='both')

plt.show()
