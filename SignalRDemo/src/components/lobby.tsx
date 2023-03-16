import { useState } from 'react';
import { Form, Button } from 'react-bootstrap';

interface Props
{
    logInUrl: string
}

const Lobby = (props: Props) => {
    const [user, setUser] = useState('');

    const logIn = (user: string) =>
    {
        fetch(props.logInUrl, {
            method: 'POST',
            headers: {
              Accept: 'application.json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({ 'name': user })
          })
    }

    return <Form className='lobby'
        onSubmit={e => {
            e.preventDefault();
            logIn(user);
        }} >
        <Form.Group>
            <Form.Control placeholder="name" onChange={e => setUser(e.target.value)} />
        </Form.Group>
        <Button variant="success" type="submit" disabled={!user}>Join</Button>
    </Form>
}

export default Lobby;
