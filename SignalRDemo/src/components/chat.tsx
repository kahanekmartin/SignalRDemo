import { SendMessageForm } from './messageForm'
import { Messages } from './messages'
import { useConnection } from '../hooks/useConnection'

interface Props 
{
    hubUrl: string
    loggedUser: User
    stream: boolean
}

interface User
{
    id: string
    name: string
    messages: Message[]
}

interface Message
{
    timestamp: Date
    content: string
    source: Source
}

export const enum Source
{
    USER = 1, 
    CHAT = 0
}

export type TMessage = Message
export type TUser = User

export const Chat = (props: Props) => 
{
    const { sendMessage, messages } = useConnection(props.hubUrl, props.loggedUser.id, props.stream)

    console.log('Chat loaded')
    
    return <>
        <div className='chat'>
            <Messages messages={messages} loggedUser={props.loggedUser} />
            <SendMessageForm loggedUser={props.loggedUser} sendMessage={sendMessage} />
        </div>
    </>
}
