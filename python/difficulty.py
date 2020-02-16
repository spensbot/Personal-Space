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
# People should start surviving 1 minute after a total of 10 minutes play.

# 2 minutes
# Reached after 1 hour of play.

# 3 minutes
# Reached after many (10?) hours of play.

# 5 minutes
# Only incredibly skilled players should reach 5 minutes. And even then only on their best games.

# infinity
# No matter how good someone gets, they should never be able to play forever.
# The game should 'asymptote' to a humanly impossible level of difficulty.

import numpy as np
import matplotlib.pyplot as plt

minutes = np.arange(0, 10, 0.01);

#---------------     ORIGINAL     --------------
_startPlayerSpeed = 6 #units per second
_startEnemySpeed = 3 #units per second
_startEnemySpawn = 2 #seconds per spawn

_modPlayerSpeed = 0.2 #change per minute
_modEnemySpeed = 0.2 #change per minute
_modEnemySpawn = -0.2 #change per minute

# y = mx + b
def linear(x, m, b):
    return m * x + b

_playerSpeed = linear(minutes, _modPlayerSpeed, _startPlayerSpeed)
_enemySpeed = linear(minutes, _modEnemySpeed, _startEnemySpeed)
_enemySpawn = linear(minutes, _modEnemySpawn, _startEnemySpawn)

#---------------     PROPOSED     -----------------


def asymptotic(x, asymptote, xShift, yScale, power):
  return yScale * 1/(x + xShift)**power + asymptote

playerSpeed = asymptotic(minutes, 10, 1.5, -4, .75)
enemySpeed = asymptotic(minutes, 6, 1.5, -3, .75)
enemySpawn = asymptotic(minutes, 0.5, 2, 3, 1)


fig, (ax1, ax2, ax3) = plt.subplots(3, sharex=True)
fig.suptitle('Difficulty Parameters over Time')
ax1.plot(minutes, _playerSpeed, 'k:')
ax1.plot(minutes, playerSpeed, 'k-')
ax1.set(ylabel='Player Speed')
ax2.plot(minutes, _enemySpeed, 'k:')
ax2.plot(minutes, enemySpeed, 'k-')
ax2.set(ylabel='Enemy Speed')
ax3.plot(minutes, _enemySpawn, 'k:')
ax3.plot(minutes, enemySpawn, 'k-')
ax3.set(ylabel='Enemy Spawn (s)', xlabel='Minutes of Play')
ax1.grid(b=True, which='both', axis='both')
ax2.grid(b=True, which='both', axis='both')
ax3.grid(b=True, which='both', axis='both')
plt.show()
