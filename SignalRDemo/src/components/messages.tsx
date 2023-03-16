import { useEffect, useRef } from 'react'

import { TMessage } from './chat'

interface Props
{
    messages: TMessage[]
}

export const Messages = ({messages} : Props) => {
    const messageRef = useRef<HTMLDivElement>();

    useEffect(() => {
        if (messageRef && messageRef.current) {
            const { scrollHeight, clientHeight } = messageRef.current;
            messageRef.current.scrollTo({ left: 0, top: scrollHeight - clientHeight, behavior: 'smooth' });
        }
    }, [messages]);

    return <div ref={messageRef} className='message-container' >
        {messages.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.content}</div>
                {/* <div className='from-user'>{m.user}</div> */}
            </div>
        )}
    </div>
}
