import { Form, Button, FormControl, InputGroup } from 'react-bootstrap';
import { useState } from 'react';
import { TUser } from './chat';

interface Props
{
    sendMessage: (userId: string, message: string) => void
    loggedUser: TUser
}

const SendMessageForm = ({ sendMessage, loggedUser }: Props) => {
    const [message, setMessage] = useState('');

    return <Form
        onSubmit={e => {
            e.preventDefault();
            sendMessage(loggedUser.id, message);
            setMessage('');
        }}>
        <InputGroup>
            <FormControl type="user" placeholder="message..."
                onChange={e => setMessage(e.target.value)} value={message} />
            <InputGroup>
                <Button variant="primary" type="submit" disabled={!message}>Send</Button>
            </InputGroup>
        </InputGroup>
    </Form>
}

export default SendMessageForm;
