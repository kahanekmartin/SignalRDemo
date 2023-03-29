import { Chat } from './chat'
import { Lobby } from './lobby'

export const components = {
    Chat,
    Lobby,
};

try {
  module.exports = components;
} catch {}