import { Button, InputGroup, Form, FormControl } from 'react-bootstrap';
import { useState } from 'react';
import { TUser } from './chat';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons'

interface Props {
    sendMessage: (message: string) => void
    loggedUser: TUser
}

export const SendMessageForm = ({ sendMessage }: Props) => {
    const [message, setMessage] = useState('');

    return <Form className="message-form"
        onSubmit={e => {
            e.preventDefault();
            sendMessage(message);
            setMessage('');
        }}>
        <InputGroup className="input">
            <FormControl type="user" placeholder="message..."
                onChange={e => setMessage(e.target.value)} value={message} />
            <InputGroup>
                <Button variant="primary" type="submit" disabled={!message}>
                    <FontAwesomeIcon icon={faPaperPlane} />
                </Button>
            </InputGroup>
        </InputGroup>
    </Form>
};