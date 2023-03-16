import SendMessageForm from './messageForm'
import { Messages } from './messages'
import { useConnection } from '../hooks/useConnection'

interface Props 
{
    hubUrl: string
    loggedUser: User
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
}

export type TMessage = Message
export type TUser = User

const Chat = (props: Props) => 
{
    const { sendMessage, messages } = useConnection(props.hubUrl)
    
    return <div>
        <div className='chat'>
            <Messages messages={messages} />
            <SendMessageForm loggedUser={props.loggedUser} sendMessage={sendMessage} />
        </div>
    </div>
}

export default Chat;
