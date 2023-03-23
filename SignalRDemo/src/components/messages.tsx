import { useEffect, useRef } from 'react'

import { TMessage, Source, TUser } from './chat'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faRobot } from '@fortawesome/free-solid-svg-icons'

interface Props
{
    messages: TMessage[]
    loggedUser: TUser
}

export const Messages = ({messages, loggedUser} : Props) => {
    const messageRef = useRef<HTMLDivElement>();

    useEffect(() => {
        if (messageRef && messageRef.current) {
            const { scrollHeight, clientHeight } = messageRef.current;
            messageRef.current.scrollTo({ left: 0, top: scrollHeight - clientHeight, behavior: 'smooth' });
        }
    }, [messages]);

    console.log(messages)

    return <div ref={messageRef} className='messages' >
        {messages.map((m, index) =>
            m.source ===  Source.USER ?
                <div key={index} className='message user'>
                    <div className='bg-primary icon' style={{background: "green"}} >{loggedUser.name.slice(0,2).toUpperCase()}</div>
                    <div className='bg-primary content'>{m.content}</div>
                </div> :
                <div key={index} className='message robot'>
                    <div className='bg-primary icon'>
                        <FontAwesomeIcon icon={faRobot} />
                    </div>
                    <div className='bg-primary content'>{m.content}</div>
                </div>
        )}
    </div>
}
